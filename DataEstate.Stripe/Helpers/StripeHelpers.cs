using System;
using DataEstate.Stripe.Enums;
namespace DataEstate.Stripe.Helpers
{
    public static class StripeHelpers
    {
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
    }
}
