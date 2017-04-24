using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class CspController : Controller
    {
        // GET: Csp
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult ImageSource(string src = "*")
        {
            Response.AddHeader("Content-Security-Policy", String.Format("image-src '{0}'", src));
            ViewBag.Source = src;
            return View();
        }
    }
}