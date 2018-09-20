using System;
using Newtonsoft.Json;

namespace DataEstate.Stripe.Models.Dtos
{
    public class InvoiceItem : SubscriptionItem
    {
        [JsonProperty("subscriptionId")]
        public string SubscriptionId;

        [JsonProperty("amount")]
        public Decimal Amount;
    }
}
