using System;
namespace DataEstate.Stripe.Enums
{
    public enum SubscriptionInterval
    {
        None = 0,
        Day = 1,
        Week = 7,
        Month = 30,
        Year = 365
    }

    public enum SubscriptionStatus
    {
        Inactive = 0, 
        Active = 1, 
        Trial = 2, 
        Unpaid = -1,
        Overdue = -2
    }

    public enum BillingType
    {
        Automatic = 0,
        Manual = 1 //Equivelent to Stripe's send invoice
    }
}
