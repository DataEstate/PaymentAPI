using System;
using DataEstate.Stripe.Models;

namespace DataEstate.Stripe.Interfaces
{
    public interface ICustomerService
    {
        Customer CreateCustomer(Customer customer);
    }
}
