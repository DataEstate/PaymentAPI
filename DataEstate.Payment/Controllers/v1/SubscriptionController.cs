using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DataEstate.Stripe.Interfaces;
using DataEstate.Stripe.Models.Dtos;
using DataEstate.Stripe.Extensions;
using DataEstate.Helpers;
using System.Net;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DataEstate.Payment.Controllers
{
    [Route("v1/[controller]")]

    public class SubscriptionController : Controller
    {
        private JsonSerializerSettings _defaultSettings;
        private ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
            _defaultSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
        }
        // GET: /<controller>/
        [Authorize("subscription:read")]
        public IActionResult Index()
        {
            return Json(new
            {
                Value = "Hello"
            });
        }

        /// <summary>
        /// Creates a subscription invitation
        /// </summary>
        /// <returns>The invitation.</returns>
        [Route("invitation")]
        [HttpPost()]
        public IActionResult CreateInvitation([FromBody] SubscriptionProposal subscriptionProposal)
        {
            var invitationString = WebUtility.UrlEncode(EncryptionHelper.EncryptObject(subscriptionProposal));
            return Json(new
            {
                String = invitationString
            }, _defaultSettings);
        }

        [Route("invitation")]
        [HttpGet()]
        public IActionResult ReadInvitation([FromQuery(Name = "id")] string encryptionString)
        {
            var invitationJson = EncryptionHelper.DecryptString(encryptionString);
            var proposal = JsonConvert.DeserializeObject<SubscriptionProposal>(invitationJson);
            return Json(proposal, _defaultSettings);
        }
    }
}
