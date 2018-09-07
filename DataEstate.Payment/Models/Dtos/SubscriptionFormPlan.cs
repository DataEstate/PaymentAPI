using System;
using Microsoft.AspNetCore.Mvc;

namespace DataEstate.Payment.Models.Dtos
{
    public class SubscriptionFormPlan
    {
        [FromForm(Name="id")]
        public string PlanId { get; set; }

        [FromForm(Name ="qty")]
        public int Quantity { get; set; }
    }
}
