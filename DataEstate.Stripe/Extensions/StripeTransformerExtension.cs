using System;
using System.Collections.Generic;
using Stripe;
using DataEstate.Stripe.Models.Dtos;
using DataEstate.Stripe.Helpers;
using DataEstate.Stripe.Enums;

namespace DataEstate.Stripe.Extensions
{
    public static class StripeTransformerExtension
    {
        /**** FROM STRIPE ****/
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
            //Product doesn't expand here, but can be contained. 
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
            if (stripeCustomer.Metadata != null)
            {
                if (stripeCustomer.Metadata.ContainsKey("Name"))
                {
                    customer.Name = stripeCustomer.Metadata["Name"];
                }
                if (stripeCustomer.Metadata.ContainsKey("BusinessName"))
                {
                    customer.BusinessName = stripeCustomer.Metadata["BusinessName"];
                }
                if (stripeCustomer.Metadata.ContainsKey("abn"))
                {
                    customer.Abn = stripeCustomer.Metadata["abn"];
                }
            }
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

        //TODO: Figure out usage for App Fee. currently not used. 
        public static Subscription ToSubscription(this StripeSubscription stripeSubscription)
        {
            var subscription = new Subscription
            {
                Id = stripeSubscription.Id,
                CustomerId = stripeSubscription.CustomerId, 
                BillingType = ((StripeBilling)stripeSubscription.Billing).ToBillingType(),
                InvoiceStartDate = stripeSubscription.BillingCycleAnchor,
                CancelAtPeriodEnd = stripeSubscription.CancelAtPeriodEnd,
                CancellationDate = stripeSubscription.CanceledAt,
                CreatedDate = (DateTime)stripeSubscription.Created,
                CurrentPeriodEndDate = stripeSubscription.CurrentPeriodEnd,
                CurrentPeriodStartDate = stripeSubscription.CurrentPeriodStart,
                DaysUntilDue = stripeSubscription.DaysUntilDue,
                SubscriptionEndDate = stripeSubscription.EndedAt, 
                Meta = stripeSubscription.Metadata,
                Status = StripeHelpers.ToSubscriptionStatus(stripeSubscription.Status), 
                Tax = stripeSubscription.TaxPercent == null ? 0.1M : (decimal)stripeSubscription.TaxPercent, 
                TrialStart = stripeSubscription.TrialStart,
                TrialEnd = stripeSubscription.TrialEnd
            };
            //Items
            if (stripeSubscription.Items != null && stripeSubscription.Items.TotalCount > 0)
            {
                subscription.Items = new List<SubscriptionItem>();
                foreach (var item in stripeSubscription.Items.Data)
                {
                    subscription.Items.Add(
                        new SubscriptionItem
                        {
                            Id = item.Id,
                            Plan = item.Plan.ToSubscriptionPlan(),
                            Quantity = item.Quantity
                        }
                    );
                }
            }
            return subscription;
        }

        public static PaymentInvoice ToPaymentInvoice(this StripeInvoice stripeInvoice)
        {
            var invoice = new PaymentInvoice
            {
                Id = stripeInvoice.Id,
                InvoiceNumber = stripeInvoice.Number,
                ReceiptNumber = stripeInvoice.ReceiptNumber,
                AmountDue = stripeInvoice.AmountDue.ToDecimalAmount(),
                AmountPaid = stripeInvoice.AmountPaid.ToDecimalAmount(),
                AmountRemaining = stripeInvoice.AmountRemaining.ToDecimalAmount(),
                AttemptCount = stripeInvoice.AttemptCount,
                Attempted = stripeInvoice.Attempted,
                CustomerId = stripeInvoice.CustomerId,
                ChargeId = stripeInvoice.ChargeId,
                Description = stripeInvoice.Description,
                //Todo: Discount
                DueDate = stripeInvoice.DueDate,
                HostedInvoiceUrl = stripeInvoice.HostedInvoiceUrl,
                HostedPdf = stripeInvoice.InvoicePdf,
                Subtotal = stripeInvoice.Subtotal.ToDecimalAmount(),
                Total = stripeInvoice.Total.ToDecimalAmount(),
                periodStartDate = stripeInvoice.PeriodStart,
                periodEndDate = stripeInvoice.PeriodEnd,
                Tax = stripeInvoice.Tax == null ? null : (decimal?)((int)stripeInvoice.Tax).ToDecimalAmount(),
                TaxPercent = stripeInvoice.TaxPercent
            };
            //Items
            if (stripeInvoice.StripeInvoiceLineItems != null && stripeInvoice.StripeInvoiceLineItems.TotalCount > 0)
            {
                invoice.Items = new List<InvoiceItem>();
                foreach (var lineItem in stripeInvoice.StripeInvoiceLineItems)
                {
                    if (lineItem != null)
                    {
                        invoice.Items.Add(lineItem.ToInvoiceItem()); 
                    }
                }
            }
            return invoice;
        }

        public static InvoiceItem ToInvoiceItem(this StripeInvoiceLineItem lineItem)
        {
            if (lineItem == null)
            {
                return null;
            }
            var invoiceItem = new InvoiceItem
            {
                Id = lineItem.Id,
                Quantity = lineItem.Quantity == null ? 1 : (int)lineItem.Quantity,
                Plan = lineItem.Plan.ToSubscriptionPlan(),
                CurrentPeriodEndDate = lineItem.StripePeriod.End,
                CurrentPeriodStartDate = lineItem.StripePeriod.Start,
                SubscriptionId = lineItem.SubscriptionId,
                Amount = lineItem.Amount.ToDecimalAmount()
            };
            return invoiceItem;
        }
        /**** TO STRIPE ****/
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

        public static StripeSubscriptionCreateOptions ToStripeSubscriptionCreate(this Subscription subscription)
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
            if (subscription.Items != null && subscription.Items.Count > 0)
            {
                subscriptionCreate.Items = new List<StripeSubscriptionItemOption>();
                foreach (var plan in subscription.Items)
                {
                    subscriptionCreate.Items.Add(
                        new StripeSubscriptionItemOption
                        {
                            PlanId = plan.PlanId,
                            Quantity = plan.Quantity
                        }
                    );
                }
            }
            return subscriptionCreate;
        }
    }
}
