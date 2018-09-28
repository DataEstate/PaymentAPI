using System;
using System.Collections.Generic;

namespace DataEstate.Stripe.Models.Configurations
{
    public class StripeSettings
    {
        public string PublicKey { get; set; }

        public string SecretKey { get; set; }

        public Dictionary<string, string> WebhookSecrets { get; set; }
    }
}
