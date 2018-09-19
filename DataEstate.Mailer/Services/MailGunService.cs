using System;
using DataEstate.Mailer.Interfaces;
using DataEstate.Mailer.Models;
using DataEstate.Mailer.Models.Dtos;
using DataEstate.Mailer.Models.Configurations;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http.Headers;

namespace DataEstate.Mailer.Services
{
    public class MailGunService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailGunService(IOptions<MailSettings> mailOptions)
        {
            _mailSettings = mailOptions.Value;
        }

        public MailResponse Send(MailContent mailBody)
        {
            return SendAsync(mailBody).Result;
        }

        /// <summary>
        /// Send the specified mailBody as email. 
        /// </summary>
        /// <returns>The send.</returns>
        /// <param name="mailBody">Mail body.</param>
        public async Task<MailResponse> SendAsync(MailContent mailBody)
        {
            //Make HTTP Request
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("api:" + _mailSettings.ApiKey)));
            //Mail Gun Request - Only accepts Form Data. 
            var message = GetRequestDictionary(mailBody);
            if (message == null)
            {
                return new MailResponse
                {
                    Status = "Error",
                    Id = "Send Error",
                    Message = $"Message is empty. Missing required parameter. Make sure the receivers are set. "
                };
            }
            var httpContent = new FormUrlEncodedContent(message);
            var responseTask = await client.PostAsync($"{_mailSettings.Host}/messages", httpContent);

            if (responseTask.IsSuccessStatusCode)
            {
                var content = await responseTask.Content.ReadAsStringAsync();
                return new MailResponse
                {
                    Status = "success"
                };
            }
            else
            {
                return new MailResponse
                {
                    Status = $"{responseTask.StatusCode}",
                    Id = responseTask.ReasonPhrase,
                    Message = responseTask.ReasonPhrase
                };
            }
        }

        private Dictionary<string,string> GetRequestDictionary(MailContent mailBody)
        {
            if (mailBody.Receivers == null)
            {
                return null;
            }
            var requestDict = new Dictionary<string, string>
            {
                {"from", mailBody.Senders == null ? _mailSettings.DefaultSender : String.Join(",", mailBody.Senders)}, 
                {"to", String.Join(",", mailBody.Receivers)}
            };
            requestDict["subject"] = mailBody.Subject == null ? _mailSettings.DefaultSubject : mailBody.Subject;
            if (mailBody.Html != null)
            {
                requestDict["html"] = mailBody.Html;
            }
            else
            {
                requestDict["text"] = mailBody.Text == null ? "No content" : mailBody.Text;
            }
            return requestDict;
        }
    }
}
