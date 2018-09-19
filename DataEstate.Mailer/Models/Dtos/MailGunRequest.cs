using System;
namespace DataEstate.Mailer.Models.Dtos
{
    public class MailGunRequest
    {
        public string from { get; set; }

        public string to { get; set; }

        //CC
        public string cc { get; set; }

        //BCC
        public string bcc { get; set; }

        public string subject { get; set; }

        public string text { get; set; }

        public string html { get; set; }
    }
}
