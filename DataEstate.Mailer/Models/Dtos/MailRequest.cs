using System;
using Newtonsoft.Json;

namespace DataEstate.Mailer.Models.Dtos
{
    public class MailRequest
    {
        public string sender { get; set; }

        public string receivers { get; set; }

        //CC
        public string cc { get; set; }

        //BCC
        public string bcc { get; set; }

        public string subject { get; set; }

        public string text { get; set; }

        public string html { get; set; }
    }
}
