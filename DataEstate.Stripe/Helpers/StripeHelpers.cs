using System;
using DataEstate.Stripe.Enums;
namespace DataEstate.Stripe.Helpers
{
    public static class StripeHelpers
    {
        public static StripeInterval ToStripeInterval(string intervalName)
        {
            switch (intervalName)
            {
                case "day":
                    return StripeInterval.Day;
                case "month":
                    return StripeInterval.Month;
                case "week":
                    return StripeInterval.Week;
                case "year":
                    return StripeInterval.Year;
                default:
                    return StripeInterval.None;
            }
        }
    }
}
