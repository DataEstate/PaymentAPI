using System;
using DataEstate.Mailer.Interfaces;
using DataEstate.Mailer.Models;
using DataEstate.Mailer.Models.Dtos;
using DataEstate.Mailer.Models.Configurations;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Collections.Generic;

namespace DataEstate.Mailer.Services
{
    public class MailGunService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailGunService(IOptions<MailSettings> mailOptions)
        {
            _mailSettings = mailOptions.Value;
        }

        /// <summary>
        /// Send the specified mailBody as email. 
        /// </summary>
        /// <returns>The send.</returns>
        /// <param name="mailBody">Mail body.</param>
        public MailResponse Send(MailContent mailBody)
        {
            //Make HTTP Request
            var client = new HttpClient();
            //Mail Gun Request
            //TODO: Serialize?


            return new MailResponse
            {
                Id = "Test",
                Message = "Hi"
            };
        }

        //private FormUrlEncodedContent CreateMailgunRequst(MailContent mailBody)
        //{
        //    var requestDict = new Dictionary<string, string>();
        //    requestDict["from"] = String.Join(",", mailBody.Senders);
        //    requestDict["to"] = String.Join(",", mailBody.Receivers);
        //}
    }
}
