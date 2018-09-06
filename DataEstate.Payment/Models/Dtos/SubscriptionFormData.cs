using System;
using Newtonsoft.Json;
namespace DataEstate.Payment.Models.Dtos
{
    public class SubscriptionFormData
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("email")]
        public string Email;

        [JsonProperty("cardHolder")]
        public string CardHolder;
    
        [JsonProperty("cardCountry")]
        public string CardCountry;

        [JsonProperty("currency")]
        public string Currency;

        [JsonProperty("token")]
        public string CardToken;
    }
}
