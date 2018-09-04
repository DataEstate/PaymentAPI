using System;
using System.Collections.Generic;
using Stripe;
using DataEstate.Stripe.Models;

namespace DataEstate.Stripe.Interfaces
{
    public interface ISubscriptionService
    {
        /// <summary>
        /// Get a single subscription plan. 
        /// </summary>
        /// <returns>The plan.</returns>
        /// <param name="PlanId">Plan identifier.</param>
        SubscriptionPlan GetPlan(string PlanId);

        /// <summary>
        /// Get all plans attached to a single product
        /// </summary>
        /// <returns>The plans.</returns>
        /// <param name="productId">Product identifier.</param>
        List<SubscriptionPlan> GetPlans(string productId);

        /// <summary>
        /// Get all subscription plans based on filter. 
        /// </summary>
        /// <returns>The plans.</returns>
        /// <param name="planListOptions">Stripe list options.</param>
        List<SubscriptionPlan> GetPlans(PlanListFilterOptions planListOptions);

        /// <summary>
        /// Get a subscription product
        /// </summary>
        /// <returns>The product.</returns>
        /// <param name="ProductId">Product identifier.</param>
        ServiceProduct GetProduct(string ProductId);


    }
}
