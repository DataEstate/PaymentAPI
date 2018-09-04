using System;
using Newtonsoft.Json;

namespace DataEstate.Stripe.Models.Dtos
{
    public class SubscriptionProposal
    {
        [JsonProperty("customer")]
        public Customer Customer;

        //Use this if a customer exist. 
        [JsonProperty("customerId")]
        public string CustomerId;

        [JsonProperty("subscription")]
        public Subscription Subscription;
    }
}
