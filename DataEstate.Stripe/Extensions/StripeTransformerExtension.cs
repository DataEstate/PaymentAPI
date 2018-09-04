using System;
using System.Collections.Generic;
using Stripe;
using DataEstate.Stripe.Models;
using DataEstate.Stripe.Helpers;


namespace DataEstate.Stripe.Extensions
{
    public static class StripeTransformerExtension
    {
        /**** TO STRIPE ****/
        public static SubscriptionPlan ToSubscriptionPlan(this StripePlan stripePlan)
        {
            var plan = new SubscriptionPlan
            {
                Id = stripePlan.Id,
                Name = stripePlan.Nickname,
                Amount = ((int)stripePlan.Amount).ToDecimalAmount(),
                Currency = stripePlan.Currency,
                IntervalCount = stripePlan.IntervalCount,
                Interval = StripeHelpers.ToStripeInterval(stripePlan.Interval),
                ProductId = stripePlan.ProductId
            };
            return plan;
        }

        public static Customer ToCustomer(this StripeCustomer stripeCustomer)
        {
            var customer = new Customer
            {
                Id = stripeCustomer.Id,
                Name = stripeCustomer.Description,
                Email = stripeCustomer.Email,
                DefaultSource = stripeCustomer.DefaultSourceId
            };
            return customer;
        }

        public static PlanListFilterOptions ToPlanListFilter(this StripePlanListOptions stripePlanOptions)
        {
            var planFilter = new PlanListFilterOptions
            {
                Active = stripePlanOptions.Active,
                EndingBefore = stripePlanOptions.EndingBefore,
                StartingAfter = stripePlanOptions.StartingAfter,
                ProductId = stripePlanOptions.ProductId,
                Limit = stripePlanOptions.Limit
            };
            if (stripePlanOptions.Created != null)
            {
                planFilter.CreatedBefore = stripePlanOptions.Created.LessThanOrEqual;
                planFilter.CreatedAfter = stripePlanOptions.Created.GreaterThanOrEqual;
            }
            return planFilter;
        }

        /**** FROM STRIPE ****/
        public static StripePlanListOptions ToStripePlanListOptions(this PlanListFilterOptions planFilter)
        {
            var stripePlanOption = new StripePlanListOptions
            {
                Active = planFilter.Active,
                EndingBefore = planFilter.EndingBefore,
                StartingAfter = planFilter.StartingAfter,
                ProductId = planFilter.ProductId,
                Limit = planFilter.Limit,
                Created = new StripeDateFilter
                {
                    GreaterThanOrEqual = planFilter.CreatedAfter,
                    LessThanOrEqual = planFilter.CreatedBefore
                }
            };
            return stripePlanOption;
        }

        public static StripeSubscriptionCreateOptions ToStripeSubscriptionCreate(this Subscription subscription, List<SubscriptionItem> subscriptionItems = null)
        {
            //TODO: Discount
            var subscriptionCreate = new StripeSubscriptionCreateOptions
            {
                CustomerId = subscription.CustomerId,
                ApplicationFeePercent = subscription.AppFee,
                Billing = subscription.BillingType.ToStripeBillingType(),
                BillingCycleAnchor = subscription.InvoiceStartDate,
                DaysUntilDue = subscription.DaysUntilDue,
                Metadata = subscription.Meta,
                Prorate = subscription.Prorate,
                TaxPercent = subscription.Tax,
                TrialEnd = subscription.TrialEnd,
                TrialPeriodDays = subscription.TrialDays
            };
            ////Plans
            if (subscriptionItems != null && subscriptionItems.Count > 0)
            {
                subscriptionCreate.Items = new List<StripeSubscriptionItemOption>();
                foreach (var plan in subscriptionItems)
                {
                    subscriptionCreate.Items.Add(
                        new StripeSubscriptionItemOption
                        {
                            PlanId = plan.Id, Quantity = plan.Quantity
                        }
                    );
                }
            }
            return subscriptionCreate;
        }
    
    }
}
