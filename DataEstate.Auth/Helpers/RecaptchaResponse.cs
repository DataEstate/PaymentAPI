using System;
namespace DataEstate.Auth.Helpers.Recaptcha
{
    public class RecaptchaResponse
    {
        public bool Success { get; set; }
        public string Challenge_ts { get; set; }
        public string Hostname { get; set; }
    }
}
