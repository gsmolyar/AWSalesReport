using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AWSalesReport.Models;

namespace AWSalesReport.Controllers
{
    public class OrderDetailsController : Controller
    {
        // GET: OrderDetails
        public ActionResult OrderDetails()
        {
            return View();
        }
    }
}