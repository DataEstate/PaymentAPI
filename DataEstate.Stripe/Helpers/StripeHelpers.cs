using System;
using System.Collections.Generic;
using DataEstate.Stripe.Enums;
namespace DataEstate.Stripe.Helpers
{
    public static class StripeHelpers
    {
        private static Dictionary<string, string> webhookSecrets;

        public static SubscriptionInterval ToStripeInterval(string intervalName)
        {
            switch (intervalName)
            {
                case "day":
                    return SubscriptionInterval.Day;
                case "month":
                    return SubscriptionInterval.Month;
                case "week":
                    return SubscriptionInterval.Week;
                case "year":
                    return SubscriptionInterval.Year;
                default:
                    return SubscriptionInterval.None;
            }
        }

        public static SubscriptionStatus? ToSubscriptionStatus(string stripeStatus)
        {
            switch (stripeStatus)
            {
                case "trialing":
                    return SubscriptionStatus.Trial;
                case "active":
                    return SubscriptionStatus.Active;
                case "past_due":
                    return SubscriptionStatus.Overdue;
                case "canceled":
                    return SubscriptionStatus.Inactive;
                case "unpaid":
                    return SubscriptionStatus.Unpaid;
                default:
                    return null;
            }
        }

        public static void SetWebhookSecret(string key, string secret)
        {
            if (webhookSecrets == null)
            {
                webhookSecrets = new Dictionary<string, string>();
            }
            webhookSecrets[key] = secret;
        }

        public static void SetWebhookSecrets(Dictionary<string, string> secrets)
        {
            webhookSecrets = secrets;
        }

        public static string GetWebhookSecret(string key)
        {
            if (webhookSecrets.ContainsKey(key))
            {
                return webhookSecrets[key];
            }
            else
            {
                return null;
            }
        }
    }
}
