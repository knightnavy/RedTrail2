using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RedTrailMVC.Controllers
{
    [Authorize]
    public class DonorController : Controller
    {
        // GET: Vendor
        public ActionResult Index()
        {
            return View();
        }
    }
}