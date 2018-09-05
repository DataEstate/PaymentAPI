using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using DataEstate.Stripe.Enums;

namespace DataEstate.Stripe.Models.Dtos
{
    public class SubscriptionPlan
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("amount")]
        public decimal Amount;

        [JsonProperty("metadata")]
        public string Metadata;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("intervalCounter")]
        public int IntervalCount;

        [JsonProperty("interval")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SubscriptionInterval Interval;

        //TODO: Enum?
        [JsonProperty("currency")]
        public string Currency;

        [JsonProperty("productId")]
        public string ProductId;

        [JsonProperty("product")]
        public SubscriptionProduct Product;
    }
}
