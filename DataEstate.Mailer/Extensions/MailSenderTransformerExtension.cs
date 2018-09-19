using System;
using DataEstate.Mailer.Models.Dtos;
using DataEstate.Mailer.Enums;

namespace DataEstate.Mailer.Extensions
{
    public static class MailSenderTransformerExtension
    {
        //TODO: Incomplete...
        public static MailGunRequest ToMailGunRequest(this MailContent mailContent)
        {
            var request = new MailGunRequest
            {
                from = mailContent.Senders == null ? "" : String.Join(",", mailContent.Senders),
                to = mailContent.Receivers == null ? "" : String.Join(",", mailContent.Receivers)
            };
            //Subject

            //Body
            if (mailContent.Html != null)
            {
                request.text = mailContent.Html;
            }
            else if (mailContent.Text != null)
            {
                request.text = mailContent.Text;
            }

            return request;
        }
    }
}
