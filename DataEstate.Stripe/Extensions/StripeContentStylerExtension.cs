using System;
using System.Globalization;
using DataEstate.Stripe.Models.Dtos;
using DataEstate.Stripe.Enums;
using Stripe;
using System.Text;

namespace DataEstate.Stripe.Extensions
{
    public static class StripeContentStylerExtension
    {
        //Quick Strings
        public static string ToFriendlyIntervalPhrase(this SubscriptionPlan plan)
        {
            string interval;

            switch (plan.Interval)
            {
                case SubscriptionInterval.Day:
                    if (plan.IntervalCount > 1)
                    {
                        interval = $"every {plan.IntervalCount} days";
                    }
                    else
                    {
                        interval = "daily";
                    }
                    break;
                case SubscriptionInterval.Month:
                    if (plan.IntervalCount > 1)
                    {
                        interval = $"every {plan.IntervalCount} months";
                    }
                    else {
                        interval = "monthly";
                    }
                    break;
                case SubscriptionInterval.Year:
                    if (plan.IntervalCount > 1)
                    {
                        interval = $"every {plan.IntervalCount} years"; 
                    }
                    else
                    {
                        interval = "annually";
                    }
                    break;
                default:
                    interval = "";
                    break;
            }
            return interval;
        }

        //AMOUNT TRANSFORMERS
        public static int ToStripeAmount(this decimal chargeAmount)
        {
            return decimal.ToInt32(chargeAmount * 100);
        }

        public static decimal ToDecimalAmount(this int stripeAmount)
        {
            var decimalAmount = (decimal)stripeAmount / 100;
            return decimalAmount;
        }

        public static string ToDollarAmount(this int stripeAmount, Boolean formatted = false)
        {
            var dollarAmount = (decimal)stripeAmount / 100;
            if (formatted)
            {
                return dollarAmount.ToString("C", CultureInfo.CurrentCulture);
            }
            else
            {
                return dollarAmount.ToString();
            }
        }
        public static string ToDollarAmount(this decimal dollarAmount, Boolean formatted = false)
        {
            if (formatted)
            {
                return dollarAmount.ToString("C", new CultureInfo("en-AU"));
            }
            else
            {
                return dollarAmount.ToString();
            }
        }
    
        public static StripeBilling ToStripeBillingType(this BillingType billingType)
        {
            switch (billingType)
            {
                case BillingType.Automatic:
                    return StripeBilling.ChargeAutomatically;
                case BillingType.Manual:
                    return StripeBilling.SendInvoice;
                default:
                    goto case BillingType.Automatic;
            }
        }

        public static BillingType ToBillingType(this StripeBilling billingType)
        {
            switch (billingType)
            {
                case StripeBilling.ChargeAutomatically:
                    return BillingType.Automatic;
                case StripeBilling.SendInvoice:
                    return BillingType.Manual;
                default:
                    goto case StripeBilling.ChargeAutomatically;
            }
        }
    }
}
