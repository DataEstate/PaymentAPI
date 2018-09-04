using System;
using DataEstate.Stripe.Models.Dtos;

namespace DataEstate.Stripe.Interfaces
{
    public interface ICustomerService
    {
        Customer CreateCustomer(Customer customer);
    }
}
