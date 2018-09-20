using System;
using DataEstate.Stripe.Models.Dtos;
using DataEstate.Stripe.Enums;
using DataEstate.Stripe.Extensions;

namespace DataEstate.Payment.Models.Pages
{
    public class InvoicePageModel : PageModel
    {
        public PaymentInvoice Invoice;
    }
}
