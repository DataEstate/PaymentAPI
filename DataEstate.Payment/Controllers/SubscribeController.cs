using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataEstate.Stripe.Interfaces;
using DataEstate.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DataEstate.Stripe.Models.Dtos;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DataEstate.Payment.Controllers
{
    [Route("[controller]")]
    public class SubscribeController : Controller
    {
        private ISubscriptionService _subscriptionService;

        public SubscribeController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
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
                ViewData["Title"] = "Invitation";
                return View("Invitation", invitation);
            }
            else 
            {
                ViewData["Message"] = "If you're trying to access the subscription invitation page, you'll need a valid invitation ID to access the invites.";
                return View("404");
            }
        }
    }
}
