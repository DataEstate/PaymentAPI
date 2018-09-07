using System;
using Newtonsoft.Json;

namespace DataEstate.Stripe.Models.Dtos
{
    public class Customer
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("businessName")]
        public string BusinessName { get; set; }

        [JsonProperty("abn")]
        public string Abn { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("defaultSource")]
        public string DefaultSource { get; set; }
    }
}
