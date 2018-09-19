using System;
using System.Collections.Generic;
using DataEstate.Mailer.Models.Dtos;
using DataEstate.Mailer.Enums;

namespace DataEstate.Mailer.Extensions
{
    public static class MailSenderTransformerExtension
    {
        //TODO: Incomplete...
        public static MailRequest ToMailRequest(this MailContent mailContent)
        {
            var request = new MailRequest
            {
                sender = mailContent.Senders == null ? null : String.Join(",", mailContent.Senders),
                receivers = mailContent.Receivers == null ? null : String.Join(",", mailContent.Receivers),
                subject = mailContent.Subject,
                html = mailContent.Html,
                text = mailContent.Text,
                cc = mailContent.Copies == null ? null : String.Join(",", mailContent.Copies),
                bcc = mailContent.BlindCopies == null ? null : String.Join(",", mailContent.BlindCopies)
            };
            //Subject
            if (mailContent.Subject != null)
            {
                request.subject = mailContent.Subject;
            }
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

        public static MailContent ToMailContent(this MailRequest mailRequest)
        {
            var content = new MailContent
            {
                Senders = mailRequest.sender == null ? null : new List<string>(mailRequest.sender.Split(',')),
                Receivers = mailRequest.receivers == null ? null : new List<string>(mailRequest.receivers.Split(',')),
                Copies = mailRequest.cc == null ? null : new List<string>(mailRequest.cc.Split(',')),
                BlindCopies = mailRequest.bcc == null ? null : new List<string>(mailRequest.bcc.Split(',')),
                Text = mailRequest.text,
                Subject = mailRequest.subject,
                Html = mailRequest.html
            };
            return content;
        }
    }
}
