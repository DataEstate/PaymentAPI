using System;
using System.Collections.Generic;
using Stripe;
using DataEstate.Stripe.Models.Dtos;
using DataEstate.Stripe.Extensions;
using DataEstate.Stripe.Interfaces;
using Stripe.Infrastructure;

namespace DataEstate.Stripe.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private StripeSubscriptionService _subscriptionService;
        private StripePlanService _planService;
        private StripeProductService _productService;

        public SubscriptionService()
        {
            _subscriptionService = new StripeSubscriptionService();
            _planService = new StripePlanService();
            _productService = new StripeProductService();
        }

        public SubscriptionPlan GetPlan(string PlanId)
        {
            var stripePlan = _planService.Get(PlanId);
            return stripePlan.ToSubscriptionPlan();
        }

        public List<SubscriptionPlan> GetPlans(string productId)
        {
            var planListOption = new PlanListFilterOptions
            {
                ProductId = productId
            };
            return GetPlans(planListOption);
        }

        public List<SubscriptionPlan> GetPlans(PlanListFilterOptions planListOptions)
        {
            var stripeListOptions = planListOptions.ToStripePlanListOptions();
            var stripePlans = _planService.List(stripeListOptions);
            var subscriptionPlans = new List<SubscriptionPlan>();
            foreach (var stripePlan in stripePlans)
            {
                subscriptionPlans.Add(stripePlan.ToSubscriptionPlan());
            }
            return subscriptionPlans;
        }

        public SubscriptionProduct GetProduct(string ProductId)
        {
            //TODO: Replace with proper stripe options later. 
            var stripePlans = _planService.List(
                new StripePlanListOptions
                {
                    ProductId = ProductId
                }
            );
            var stripeProduct = _productService.Get(ProductId);
            var product = new SubscriptionProduct
            {
                Name = stripeProduct.Name,
                Plans = new List<SubscriptionPlan>(),
                Status = stripeProduct.Caption
            };
            foreach (var stripePlan in stripePlans)
            {
                product.Plans.Add(stripePlan.ToSubscriptionPlan());
            }
            return product;
        }

        public Subscription CreateSubscription(string customerId, Subscription subscription)
        {
            var subscriptionCreate = subscription.ToStripeSubscriptionCreate();
            if (subscriptionCreate.CustomerId == null)
            {
                subscriptionCreate.CustomerId = customerId;
            }
            var newSubscription = _subscriptionService.Create();
            return newSubscription.ToSubscription();
        }
    }
}
