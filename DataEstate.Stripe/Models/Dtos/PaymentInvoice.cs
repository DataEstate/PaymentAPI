using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DataEstate.Stripe.Models.Dtos
{
    public class PaymentInvoice
    {
        //Invoice ID. 
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("invoiceNumber")]
        public string InvoiceNumber;

        [JsonProperty("receipt")]
        public string ReceiptNumber;

        [JsonProperty("amountDue")]
        public Decimal AmountDue;

        [JsonProperty("amountPaid")]
        public Decimal AmountPaid;

        [JsonProperty("amountRemaining")]
        public Decimal AmountRemaining;

        [JsonProperty("attempts")]
        public int AttemptCount;

        [JsonProperty("attempted")]
        public bool Attempted;

        [JsonProperty("customer")] //This usually needs additional requests to set. 
        public Customer Customer;

        [JsonProperty("customerId")]
        public string CustomerId;

        [JsonProperty("chargeId")]
        public string ChargeId;

        [JsonProperty("description")]
        public string Description;

        //percentage. 
        [JsonProperty("discountr")]
        public Decimal Discount;

        [JsonProperty("dueDate")]
        public DateTime? DueDate;

        //Stripe Info. 
        [JsonProperty("hostedInvoice")]
        public string HostedInvoiceUrl;

        [JsonProperty("hostedInvoicePdf")]
        public string HostedPdf;

        [JsonProperty("items")]
        public List<InvoiceItem> Items;

        [JsonProperty("tax")]
        public Decimal? Tax;

        [JsonProperty("taxPercent")]
        public Decimal? TaxPercent;

        [JsonProperty("periodEnd")]
        public DateTime? periodEndDate;

        [JsonProperty("periodStart")]
        public DateTime? periodStartDate;

        [JsonProperty("statementDescriptor")]
        public string StatementDescriptor;

        [JsonProperty("paid")]
        public bool Paid;

        [JsonProperty("subtotal")]
        public Decimal Subtotal;

        [JsonProperty("total")]
        public Decimal Total;
    }
}
