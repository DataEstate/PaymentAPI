using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DataEstate.Stripe.Models.Dtos
{
    public class SubscriptionProduct
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("status")]
        public string Status;

        [JsonProperty("plans")]
        public List<SubscriptionPlan> Plans;
    }
}
