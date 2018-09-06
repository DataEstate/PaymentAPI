using System;
using Newtonsoft.Json;

namespace DataEstate.Payment.Models.Dtos
{
    public class ErrorResponse
    {
        [JsonProperty("status")]
        public string Status;

        [JsonProperty("code")]
        public int StatusCode;

        [JsonProperty("message")]
        public string Message;

        //TODO: Add stack frames
    }
}
