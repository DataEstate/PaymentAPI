using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataEstate.Auth.Helpers.Recaptcha
{
    public class RecaptchaHelper
    {
        private static string _recaptchaRequest;
        private static string _recaptchaSecret;

        /// <summary>
        /// Validate the recaptcha response by requesting Google
        /// </summary>
        /// <param name="recaptchaToken">Recaptcha response token from the widget on the client side. </param>
        /// <param name="userIp">Optional userIP</param>
        /// <returns></returns>
        public static async Task<bool> ValidateRecaptcha(string recaptchaToken, string userIp)
        {
            var client = new HttpClient();
            var requestBody = new Dictionary<string, string>
        {
            { "secret", _recaptchaSecret },
            { "response", recaptchaToken }
        };
            if (userIp != null && userIp != "")
            {
                requestBody["remoteip"] = userIp;
            }
            var httpContent = new FormUrlEncodedContent(requestBody);
            var requestTask = await client.PostAsync(_recaptchaRequest, httpContent);
            var content = await requestTask.Content.ReadAsStringAsync();
            RecaptchaResponse result = JsonConvert.DeserializeObject<RecaptchaResponse>(content);
            return result.Success;
        }

        /// Configuration methods below, please set these in Startup.cs before using the service. ///
        public static void SetRecaptchaRequest(string recaptchaRequestUrl)
        {
            _recaptchaRequest = recaptchaRequestUrl;
        }
        public static void SetSecret(string recaptchaSecret)
        {
            _recaptchaSecret = recaptchaSecret;
        }
    }
}
