using System;
using Newtonsoft.Json;

namespace DataEstate.Stripe.Models.Dtos
{
    public class SubscriptionProposal
    {
        //Invitation ID. Used to identify whether an invite has already been used. 
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("customer")]
        public Customer Customer;

        //Use this if a customer exist. 
        [JsonProperty("customerId")]
        public string CustomerId;

        [JsonProperty("subscription")]
        public Subscription Subscription;

        //How long is this invitation valid for. Null for no expiration. 
        [JsonProperty("expiry")]
        public DateTime? ExpiryDate;
    }
}
