using System;
using System.Collections.Generic;
using System.IO;
using Stripe;
using DataEstate.Mailer.Interfaces;
using DataEstate.Stripe.Interfaces;
using DataEstate.Stripe.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DataEstate.Payment.Models.Pages;
using DataEstate.Payment.Models.Dtos;
using DataEstate.Mailer.Models.Dtos;
using Microsoft.Extensions.Logging;
using DataEstate.Stripe.Helpers;
using NLog;
using Microsoft.AspNetCore.Hosting;
using System.Net;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DataEstate.Payment.Controllers.v1
{
    [Route("v1/[controller]")]
    public class WebhookController : Controller
    {
        private const string WHS_KEY = "InvoiceWebhook";
        private const string STRIPE_SIGNATURE = "Stripe-Signature";

        private JsonSerializerSettings _defaultSettings;
        private ISubscriptionService _subscriptionService;
        private ICustomerService _customerService;
        private IMailService _mailService;
        private ITemplateService _templateService;
        private ILogger<WebhookController> _logger;
        private IHostingEnvironment _env;

        public WebhookController(ISubscriptionService subscriptionService, ICustomerService customerService, 
                                 IMailService mailService, ITemplateService templateService, 
                                 ILogger<WebhookController> logger, IHostingEnvironment env)
        {
            _subscriptionService = subscriptionService;
            _customerService = customerService;
            _defaultSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            _mailService = mailService;
            _templateService = templateService;
            _logger = logger;
            _env = env;
        }

        [Route("stripe/invoice")]
        [HttpPost()]
        public IActionResult StripeInvoiceWebhook()
        {
            try
            {
                var webhookSecret = StripeHelpers.GetWebhookSecret(WHS_KEY);
                var requestBody = new StreamReader(Request.Body).ReadToEnd();
                StripeEvent eventRequest = null;
                if (Request.Headers.ContainsKey(STRIPE_SIGNATURE))
                {
                    var signature = Request.Headers[STRIPE_SIGNATURE];
                    eventRequest = StripeEventUtility.ConstructEvent(requestBody, signature, webhookSecret);
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                }
                //eventRequest = JsonConvert.DeserializeObject<StripeEvent>(requestBody);
                var eventData = eventRequest.Data.Object;
                switch (eventRequest.Type)
                {
                    case "invoice.payment_succeeded":
                        var eventInvoice = eventData.ToObject<StripeInvoice>();
                        return ProcessInvoicePaymentSucceeded(eventInvoice);
                }
                return Json(new
                {
                    StatusCode = true
                });
            }
            catch (Exception e)
            {
                var errorResponse = new ErrorResponse
                {
                    Status = "Error",
                    Message = e.Message
                };
                _logger.LogError($"{Response.StatusCode} - {e.Message}, Stack: {e.StackTrace}");
                return Json(errorResponse);
            }
        }

        private IActionResult ProcessInvoicePaymentSucceeded(StripeInvoice invoiceData)
        {
            try
            {
                var paymentInvoice = invoiceData.ToPaymentInvoice();
                //Get Customer
                if (paymentInvoice.Customer == null && paymentInvoice.CustomerId != null)
                {
                    paymentInvoice.Customer = _customerService.GetCustomer(paymentInvoice.CustomerId);
                }
                //Inly proceed if email is found. 
                var logoUrl = $"{Request.Scheme}://{Request.Host}/images/shield.png";
                var invoicePage = new InvoicePageModel
                {
                    Title = "Subscription Invoice",
                    Subtitle = "Thank you for choosing Data Estate!",
                    BrandName = "Data Estate Pty Ltd",
                    LogoUrl = logoUrl,
                    Invoice = paymentInvoice
                };
                invoicePage.Invoice.Paid = true;
                var invoiceSuccessEmail = _templateService.RenderTemplateAsync("Emails/InvoiceSucceeded", invoicePage).Result;
                var recipients = new List<string>();
                var customerName = paymentInvoice.Customer.Name == null ? "" : paymentInvoice.Customer.Name;
                recipients.Add(
                    $"{customerName} <{paymentInvoice.Customer.Email}>"
                );
                //Build Mail Object
                var mailContent = new MailContent
                {
                    Receivers = recipients,
                    Subject = $"Data Estate Subscription Invoice {paymentInvoice.InvoiceNumber}",
                    Html = invoiceSuccessEmail
                };
                return Json(_mailService.Send(mailContent), _defaultSettings);
                //return Content(invoiceSuccessEmail.Result, "text/html");
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                var errorResponse = new ErrorResponse
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = e.Message
                };
                _logger.LogError($"{e.Message} - {e.StackTrace}");
                return Json(errorResponse, _defaultSettings);
            }
        }
    }
}
