using System;
using Newtonsoft.Json;

namespace DataEstate.Stripe.Models
{
    public class SubscriptionProposal : SubscriptionPlan
    {
        [JsonProperty("quantity")]
        public int Quantity;
    }
}
