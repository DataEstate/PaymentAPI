﻿using System;
using System.Collections.Generic;
using Stripe;
using DataEstate.Stripe.Models.Dtos;

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
        SubscriptionProduct GetProduct(string ProductId);

        /// <summary>
        /// Create a subscription 
        /// </summary>
        /// <returns>The subscription.</returns>
        /// <param name="CustomerId">Customer identifier.</param>
        /// <param name="subscriptionItem">Subscription items (plans).</param>
        Subscription CreateSubscription(string customerId, List<SubscriptionItem> subscriptionItems);

        /// <summary>
        /// Creates the subscription with subscription options.
        /// </summary>
        /// <returns>The subscription.</returns>
        /// <param name="subscription">Subscription.</param>
        Subscription CreateSubscription(Subscription subscription);
    }
}
