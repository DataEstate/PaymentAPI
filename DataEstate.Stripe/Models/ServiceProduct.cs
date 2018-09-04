using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DataEstate.Stripe.Models
{
    public class ServiceProduct
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("status")]
        public string Status;

        [JsonProperty("plans")]
        public List<SubscriptionPlan> Plans;
    }
}
