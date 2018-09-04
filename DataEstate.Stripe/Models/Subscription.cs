using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using DataEstate.Stripe.Enums;

namespace DataEstate.Stripe.Models
{
    public class Subscription
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("appFee")]
        public decimal? AppFee;

        [JsonProperty("billingType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public BillingType BillingType;

        [JsonProperty("invoiceStartDate")]
        public DateTime? InvoiceStartDate;

        [JsonProperty("cancelAtPeriodEnd")]
        public bool? CancelAtPeriodEnd;

        [JsonProperty("cancelDate")]
        public DateTime? CancellationDate;

        [JsonProperty("addDate")]
        public DateTime CreatedDate;

        [JsonProperty("currentPeriodEnd")]
        public DateTime? CurrentPeriodEndDate;

        [JsonProperty("currentPeriodStart")]
        public DateTime? CurrentPeriodStartDate;

        [JsonProperty("items")]
        public List<SubscriptionItem> Items;

        [JsonProperty("customerId")]
        public string CustomerId;

        [JsonProperty("daysUntilDue")]
        public int? DaysUntilDue;

        //TODO: Discount

        [JsonProperty("endDate")]
        public DateTime? SubscriptionEndDate;

        [JsonProperty("meta")]
        public Dictionary<string, string> Meta;

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SubscriptionStatus Status;

        [JsonProperty("tax")]
        public decimal? Tax;

        [JsonProperty("trialStart")]
        public DateTime? TrialStart;

        [JsonProperty("trialEnd")]
        public DateTime? TrialEnd;

        [JsonProperty("trialDays")]
        public int? TrialDays;

        [JsonProperty("prorate")]
        public bool Prorate = true;
    }
}
