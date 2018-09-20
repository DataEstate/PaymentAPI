using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DataEstate.Payment.Models.Dtos
{
    public class SubscriptionFormData
    {
        [JsonProperty("name")]
        [FromForm(Name = "name")]
        public string Name { get; set; }

        [JsonProperty("businessName")]
        [FromForm(Name = "businessName")]
        public string BusinessName { get; set; }

        [JsonProperty("abn")]
        [FromForm(Name = "abn")]
        public string Abn { get; set; }

        [JsonProperty("email")]
        [FromForm(Name = "email")]
        public string Email { get; set; }

        [JsonProperty("cardHolder")]
        [FromForm(Name = "cardHolder")]
        public string CardHolder { get; set; }
    
        [JsonProperty("cardCountry")]
        [FromForm(Name = "cardCountry")]
        public string CardCountry { get; set; }

        [JsonProperty("currency")]
        [FromForm(Name = "currency")]
        public string Currency { get; set; }

        [JsonProperty("token")]
        [FromForm(Name = "token")]
        public string CardToken { get; set; }

        [JsonProperty("plans")]
        [FromForm(Name = "plans")]
        public List<SubscriptionFormPlan> Plans { get; set; }

        [JsonProperty("taxPercent")]
        [FromForm(Name= "taxPercent")]
        public decimal? TaxPercent { get; set; }
    }
}
