using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataEstate.Stripe.Interfaces;
using DataEstate.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DataEstate.Stripe.Models.Dtos;
using DataEstate.Payment.Models.Dtos;
using Stripe;
using DataEstate.Mailer.Models.Dtos;
using DataEstate.Mailer.Interfaces;
using DataEstate.Mailer.Extensions;
using DataEstate.Payment.Models.Pages;
using DataEstate.Stripe.Extensions;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
/***
 * This is a list of Pages for client interactions regarding subscriptions. 
 */
namespace DataEstate.Payment.Controllers
{
    [Route("v1/[controller]")]
    public class SubscribeController : Controller
    {
        private JsonSerializerSettings _defaultSettings;
        private ISubscriptionService _subscriptionService;
        private ICustomerService _customerService;
        private IMailService _mailService;
        private ITemplateService _templateService;
        private ILogger<SubscribeController> _logger;

        public SubscribeController(ISubscriptionService subscriptionService, ICustomerService customerService, IMailService mailService, ITemplateService templateService, ILogger<SubscribeController> nlogger)
        {
            _subscriptionService = subscriptionService;
            _customerService = customerService;
            _defaultSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            _mailService = mailService;
            _templateService = templateService;
            _logger = nlogger;
        }
       
        [Route("invite")]
        [HttpGet("invite")]
        public IActionResult Invite([FromQuery(Name = "id")]string inviteString = null)
        {
            if (inviteString != null)
            {
                try
                {
                    var invitationJson = EncryptionHelper.DecryptString(inviteString);
                    var invitation = JsonConvert.DeserializeObject<SubscriptionProposal>(invitationJson);
                    //Check Expiry. Cancel if expired. 
                    if (invitation.ExpiryDate != null && invitation.ExpiryDate < DateTime.Now)
                    {
                        _logger.LogInformation("Request for expired string detected");
                        ViewData["ErrorTitle"] = "Your invitation has expired. ";
                        ViewData["ErrorDescription"] = "Unfortunately this invitation has expired. Please contact Data Estate to have it sent again.";
                        return View("InvitationError");
                    }
                    //TODO: Check for invitation ID and Email, to avoid duplication. 
                    //Subscription
                    var subtotal = 0M;
                    if (invitation.Subscription != null && invitation.Subscription.Items != null)
                    {
                        foreach (var item in invitation.Subscription.Items)
                        {
                            //Find plans
                            if (item.PlanId != null && item.Plan == null)
                            {
                                item.Plan = _subscriptionService.GetPlan(item.PlanId);
                                if (item.Plan.ProductId != null)
                                {
                                    item.Plan.Product = _subscriptionService.GetProduct(item.Plan.ProductId);
                                }
                            }
                            subtotal += item.Plan.Amount * (decimal)item.Quantity;
                        }
                    }
                    var taxTotal = (invitation.Subscription.Tax / 100) * subtotal;
                    var total = Math.Round(subtotal + taxTotal, 2, MidpointRounding.AwayFromZero);
                    ViewData["Title"] = "Subscription Invitation | Data Estate Connector";
                    ViewData["BannerTitle"] = "Data Estate Connector";
                    ViewData["Subtotal"] = subtotal.ToDollarAmount(true);
                    ViewData["TaxRate"] = invitation.Subscription.Tax;
                    ViewData["Tax"] = taxTotal.ToDollarAmount(true); //Always GST for now. 
                    ViewData["Total"] = total.ToDollarAmount(true);
                    _logger.LogInformation("Invitation Viewed");
                    return View("Invitation", invitation);
                }
                catch (ArgumentException ae) {
                    Response.StatusCode = 500;
                    ViewData["ErrorTitle"] = "There's a problem with the server setup. ";
                    ViewData["ErrorDescription"] = $"There's a problem with the encryption of this invite ID on the server. Please report this issue to <a href='mailto:support@dataestate.com.au'>Data Estate</a> and wait for the team to resolve. Issue found: <br><p><em>{ae.Message}</em></p>";
                    _logger.LogError($"Encryption Error: {ae.Message}, Source: {ae.Source}, StackTrace: {ae.StackTrace}");
                    return View("InvitationError");
                }
                catch (Exception e) {
                    Response.StatusCode = 400;
                    ViewData["ErrorTitle"] = "There's a problem with your request";
                    ViewData["ErrorDescription"] = $"Following problems have been found with your request: {e.Message}";
                    _logger.LogError($"Request Error: {e.Message}, Source: {e.Source}, StackTrace: {e.StackTrace}");
                    return View("InvitationError");
                }
            }
            else 
            {
                ViewData["Message"] = "If you're trying to access the subscription invitation page, you'll need a valid invitation ID to access the invites.";
                return View("404");
            }
        }

        /*
        [Route("invite/send")]
        [HttpPost()]
        public async Task<IActionResult> SendInvite([FromBody] MailRequest mailRequest)
        {
            var logoUrl = $"{Request.Scheme}://{Request.Host}/images/shield.png";
            var pageModel = new PageModel
            {
                Title = "Subscription Receipt",
                Subtitle = "Thank you for subscribing to Data Estate.",
                BrandName = "Data Estate Pty Ltd",
                LogoUrl = logoUrl
            };
            var emailString = await _templateService.RenderTemplateAsync("E/SubscriptionReceipt", pageModel);

            var mailContent = mailRequest.ToMailContent();
            mailContent.Html = emailString;
            //return Content(emailString, "text/html");
            var mailResponse = await _mailService.SendAsync(mailContent);
            return Json(mailResponse, _defaultSettings);
        }*/

        [Route("invite")]
        [HttpPost()]
        [ValidateAntiForgeryToken]
        public IActionResult InviteSubmission([FromForm] SubscriptionFormData subscriptionFormData)
        {
            //TODO: Move to service or helper?
            /** Process Plans **/
            //Stop if no plans to subscribe. 
            if (subscriptionFormData.Plans == null || subscriptionFormData.Plans.Count <= 0)
            {
                Response.StatusCode = 400;
                var errMessage = "Submission had no plans attached. No transaction or subscription occured. ";
                ViewData["ErrorTitle"] = "There's a problem with your request";
                ViewData["ErrorDescription"] = errMessage;
                _logger.LogInformation(errMessage);
                return View("InvitationError");
            }
            var plans = new List<SubscriptionItem>();
            foreach (var planData in subscriptionFormData.Plans)
            {
                plans.Add(
                    new SubscriptionItem
                    {
                        PlanId = planData.PlanId,
                        Quantity = planData.Quantity
                    }
                );
            }
            Customer customer;
            /** Create Customer. **/
            if (subscriptionFormData.Email == null)
            {
                var errMessage = "New subscription will require a valid client email. No transaction or subscription occured. ";
                Response.StatusCode = 400;
                ViewData["ErrorTitle"] = "There's a problem with your request";
                ViewData["ErrorDescription"] = errMessage;
                _logger.LogInformation(errMessage);
                return View("InvitationError");
            }
            if (subscriptionFormData.CardToken == null)
            {
                var errMessage = "Subscription requires a valid card token. No transaction or subscription occured. ";
                Response.StatusCode = 400;
                ViewData["ErrorTitle"] = "There's a problem with your request";
                ViewData["ErrorDescription"] = errMessage;
                _logger.LogInformation(errMessage);
                return View("InvitationError");
            }
            try {
                var customerCreate = new Customer
                {
                    Email = subscriptionFormData.Email,
                    DefaultSource = subscriptionFormData.CardToken
                };
                if (subscriptionFormData.BusinessName != null)
                {
                    customerCreate.BusinessName = subscriptionFormData.BusinessName;
                }
                if (subscriptionFormData.Name != null)
                {
                    customerCreate.Name = subscriptionFormData.Name;
                }
                if (subscriptionFormData.Abn != null)
                {
                    customerCreate.Abn = subscriptionFormData.Abn;
                }
                customer = _customerService.CreateCustomer(customerCreate);
            }
            catch (StripeException se)
            {
                _logger.LogInformation("Stripe Error: " + se.Message);
                if (se.StripeError.DeclineCode != null)
                {
                    ViewData["ErrorTitle"] = "There was a problem with your card.";
                    ViewData["ErrorDescription"] = $"{se.Message} Your transaction did not go through. Please check with your card provider and try again later.";
                    return View("InvitationError");
                }
                else
                {
                    ViewData["ErrorTitle"] = "There's a problem with your payment request";
                }
                switch (se.StripeError.Code)
                {
                    case "token_already_used":
                        ViewData["ErrorDescription"] = "It seems that the card that was used for this transaction has already been used and it's likely that your payment has gone through. " +
                            "It is likely that you tried to refresh this page after your payment has been made. If you have any problems, please contact Data Estate.";
                        break;
                    default:
                        ViewData["ErrorDescription"] = se.Message;
                        break;
                }

                return View("InvitationError");
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Exception: {e.Message}, Source:{e.Source}, StackTrace: {e.StackTrace}");
                ViewData["ErrorTitle"] = "There's a problem with your request";
                ViewData["ErrorDescription"] = e.Message;
                return View("InvitationError");
            }
            /** Create subscription. **/
            if (customer == null)
            {
                Response.StatusCode = 400;
                ViewData["ErrorTitle"] = "There's a problem with your request";
                ViewData["ErrorDescription"] = "Client creation failed, and therefore was not able to proceed with subscription. No transaction or subscription occured. ";
                _logger.LogInformation("Request had no customer information");
                return View("InvitationError");
            }
            var subscriptionCreate = new Subscription
            {
                CustomerId = customer.Id,
                Items = plans, 
                Tax = subscriptionFormData.TaxPercent == null ? 10M : (decimal)subscriptionFormData.TaxPercent
            };
                
            var subscription = _subscriptionService.CreateSubscription(subscriptionCreate);
            _logger.LogInformation($"Subscription success. Customer ID: {customer.Id}, subscription ID: {subscription.Id}");
            return View("InvitationSubmitted", subscription);
        }


    }
}
