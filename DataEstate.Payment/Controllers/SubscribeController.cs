using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataEstate.Stripe.Interfaces;
using DataEstate.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using DataEstate.Stripe.Models.Dtos;
using DataEstate.Payment.Models.Dtos;
using System.IO;
using Microsoft.Extensions.Primitives;
using Stripe;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DataEstate.Payment.Controllers
{
    [Route("[controller]")]
    public class SubscribeController : Controller
    {
        private JsonSerializerSettings _defaultSettings;
        private ISubscriptionService _subscriptionService;
        private ICustomerService _customerService;

        public SubscribeController(ISubscriptionService subscriptionService, ICustomerService customerService)
        {
            _subscriptionService = subscriptionService;
            _customerService = customerService;
            _defaultSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [Route("invite")]
        [HttpGet("invite")]
        public IActionResult Invite([FromQuery(Name = "id")]string inviteString = null)
        {
            if (inviteString != null)
            {
                var invitationJson = EncryptionHelper.DecryptString(inviteString);
                var invitation = JsonConvert.DeserializeObject<SubscriptionProposal>(invitationJson);

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
                    }
                }
                ViewData["Title"] = "Subscription Invitation | Data Estate Connector";
                ViewData["BannerTitle"] = "Data Estate Connector";
                return View("Invitation", invitation);
            }
            else 
            {
                ViewData["Message"] = "If you're trying to access the subscription invitation page, you'll need a valid invitation ID to access the invites.";
                return View("404");
            }
        }

        [Route("invite")]
        [HttpPost()]
        public IActionResult InviteSubmission()
        {
            var formData = new StreamReader(Request.Body).ReadToEnd();
            var queryData = QueryHelpers.ParseQuery(formData);

            //TODO: Move to service or helper?
            /** Process Plans **/
            //Stop if no plans to subscribe. 
            if (!queryData.ContainsKey("plans[]"))
            {
                Response.StatusCode = 400;
                return Json(new ErrorResponse
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = "Request had no plans to subscribe to. "
                }, _defaultSettings);
            }

            var plansData = queryData["plans[]"];
            var plans = new List<SubscriptionItem>();
            foreach (var planData in plansData)
            {
                var planContent = planData.Split(":"); //First is plan ID, second is quantity. 
                plans.Add(
                    new SubscriptionItem
                    {
                        PlanId = planContent[0], 
                        Quantity = Convert.ToInt16(planContent[1])
                    }
                );
            }

            Customer customer;
            /** Create Customer. **/
            if (!queryData.ContainsKey("email"))
            {
                return Json(new ErrorResponse
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = "Request missing client email."
                });
            }
            if (!queryData.ContainsKey("token"))
            {
                return Json(new ErrorResponse
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = "Card Token is required. "
                });
            }
            try {
                var customerCreate = new Customer
                {
                    Email = queryData["email"],
                    DefaultSource = queryData["token"]
                };
                if (queryData.ContainsKey("name"))
                {
                    customerCreate.Name = queryData["name"];
                }
                customer = _customerService.CreateCustomer(customerCreate);
            }
            catch (StripeException se)
            {
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
                ViewData["ErrorTitle"] = "There's a problem with your request";
                ViewData["ErrorDescription"] = e.Message;
                return View("InvitationError");
            }
            /** Create subscription. **/
            if (customer == null)
            {
                return Json(new ErrorResponse
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = "No client present. Possibly failed creation."
                }, _defaultSettings);
            }
            var subscription = _subscriptionService.CreateSubscription(customer.Id, plans);
            return View("InvitationSubmitted", subscription);
        }
    }
}
