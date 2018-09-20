using System;
using System.Collections.Generic;
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

            //Metadata
            var metadata = new Dictionary<string, string>();
            if (customer.Name != null)
            {
                metadata["Name"] = customer.Name;
            }
            if (customer.BusinessName != null)
            {
                metadata["BusinessName"] = customer.BusinessName;
            }
            if (customer.Abn != null)
            {
                metadata["ABN"] = customer.Abn;
            }
            if (metadata.Count > 0)
            {
                createOptions.Metadata = metadata;
            }
            var stripeCustomer = _customerService.Create(createOptions);
            return stripeCustomer.ToCustomer();
        }

        public Customer GetCustomer(string customerId)
        {
            return _customerService.Get(customerId).ToCustomer();
        }
    }
}
