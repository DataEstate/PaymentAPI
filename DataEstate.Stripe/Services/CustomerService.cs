using System;
using DataEstate.Stripe.Interfaces;
using DataEstate.Stripe.Models.Dtos;
using DataEstate.Stripe.Extensions;
using Stripe;

namespace DataEstate.Stripe.Services
{
    public class CustomerService:ICustomerService
    {
        private StripeCustomerService _customerService;
        public CustomerService()
        {
            _customerService = new StripeCustomerService();
        }

        public Customer CreateCustomer(Customer customer)
        {
            var createOptions = new StripeCustomerCreateOptions
            {
                Description = customer.Name,
                Email = customer.Email
            };
            if (customer.DefaultSource != null)
            {
                createOptions.SourceToken = customer.DefaultSource;
            }
            var stripeCustomer = _customerService.Create(createOptions);
            return stripeCustomer.ToCustomer();
        }
    }
}
