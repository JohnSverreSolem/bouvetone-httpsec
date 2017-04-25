using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public ActionResult ImageSource(CspSource src = CspSource.None)
        {
            Response.AddHeader("Content-Security-Policy", String.Format("img-src {0};", GetSource(src, "media.giphy.com")));
            ViewBag.Source = src;

            return View();
        }

        [Flags]
        public enum CspSource
        {
            None = 1,
            Self = 2,
            Url = 4,
            Data = 8,
            All = 16,
            SelfUrl = Self | Url,
            SelfData = Self | Data,
            SelfDataUrl  = Self | Data | Url
        }

        private string GetSource(CspSource source, string url = "www.example.com")
        {
            List<string> sources = new List<string>();

            if (source.HasFlag(CspSource.None))
            {
                return "'none'";
            }

            if (source.HasFlag(CspSource.All))
            {
                return "'*'";
            }

            if (source.HasFlag(CspSource.Self))
            {
                sources.Add("'self'");
            }

            if (source.HasFlag(CspSource.Url))
            {
                sources.Add(url);
            }

            if (source.HasFlag(CspSource.Data))
            {
                sources.Add("data:");
            }

            return String.Join(" ", sources);
        }
    }
}