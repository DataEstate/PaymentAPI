using System;
using Newtonsoft.Json;

namespace DataEstate.Stripe.Models.Dtos
{
    public class SubscriptionItem
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("quantity")]
        public int Quantity = 1;

        [JsonProperty("planId")]
        public string PlanId;

        [JsonProperty("plan")]
        public SubscriptionPlan Plan;
    }
}
