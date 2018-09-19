using System;
using DataEstate.Mailer.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DataEstate.Mailer.Models.Configurations
{
    public class MailSettings
    {
        
        public MailerType Type { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool UseSSL { get; set; }
        public string ApiKey { get; set; }
        public string DefaultSender { get; set; }
        public string DefaultMessage { get; set; }
        public string DefaultSubject { get; set; }
    }
}
