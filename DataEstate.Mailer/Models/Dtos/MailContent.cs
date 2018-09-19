using System;
using System.Collections.Generic;
using DataEstate.Mailer.Enums;

namespace DataEstate.Mailer.Models.Dtos
{
    public class MailContent
    {
        public List<string> Senders { get; set; }

        public List<string> Receivers { get; set; }

        //CC
        public List<string> Copies { get; set; }

        //BCC
        public List<string> BlindCopies { get; set; }

        public string Subject { get; set; }

        public string Text { get; set; }

        public string Html { get; set; }

        //Mailing service type
        public MailerType Type { get; set; }

        //TODO: Attachment
    }
}
