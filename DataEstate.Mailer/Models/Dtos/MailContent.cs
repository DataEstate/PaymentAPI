using System;
using System.Collections.Generic;
using DataEstate.Mailer.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DataEstate.Mailer.Models.Dtos
{
    public class MailContent
    {
        [JsonProperty("senders")]
        public List<string> Senders { get; set; }

        [JsonProperty("receivers")]
        public List<string> Receivers { get; set; }

        //CC
        [JsonProperty("cc")]
        public List<string> Copies { get; set; }

        //BCC
        [JsonProperty("bcc")]
        public List<string> BlindCopies { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("html")]
        public string Html { get; set; }

        //Mailing service type
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public MailerType Type { get; set; }

        //TODO: Attachment
    }
}
