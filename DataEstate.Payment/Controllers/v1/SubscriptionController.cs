using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DataEstate.Payment.Controllers
{
    [Route("v1/[controller]")]

    public class SubscriptionController : Controller
    {
        // GET: /<controller>/
        [Authorize("subscription:read")]
        public IActionResult Index()
        {
            return Json(new
            {
                Value = "Hello"
            });
        }
    }
}
