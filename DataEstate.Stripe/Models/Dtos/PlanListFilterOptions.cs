using System;
using Newtonsoft.Json;

namespace DataEstate.Stripe.Models.Dtos
{
    public class PlanListFilterOptions
    {
        [JsonProperty("active")]
        public bool? Active;

        [JsonProperty("createdBefore")]
        public DateTime? CreatedBefore;

        [JsonProperty("createdAfter")]
        public DateTime? CreatedAfter;

        [JsonProperty("limit")]
        public int? Limit;

        [JsonProperty("productId")]
        public string ProductId;

        [JsonProperty("startAfter")]
        public string StartingAfter;

        [JsonProperty("endBefore")]
        public string EndingBefore;
    }
}
