using System;
using Newtonsoft.Json;

namespace DataEstate.Stripe.Models
{
    public class Customer
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("defaultSource")]
        public string DefaultSource { get; set; }
    }
}
