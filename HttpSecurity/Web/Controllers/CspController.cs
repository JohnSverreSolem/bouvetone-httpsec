using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Web.Models;

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
            string source = GetSource(src, "media.giphy.com");
            Response.AddHeader("Content-Security-Policy", String.Format("img-src {0};", source));
            ViewBag.Source = source;

            return View();
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult ReportOnly()
        {
            var header = "Content-Security-Policy-Report-Only";
            var policy = "default-src 'self'; report-uri /Csp/RegisterReport";
            Response.AddHeader(header, policy);

            ViewBag.HttpHeader = header;
            ViewBag.Policy = policy;

            return View();
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Report()
        {
            var header = "Content-Security-Policy-Report";
            var policy = "default-src 'self'; report-uri /Csp/RegisterReport";
            Response.AddHeader(header, policy);

            ViewBag.HttpHeader = header;
            ViewBag.Policy = policy;

            return View();
        }

        [HttpGet]
        public ActionResult DownloadCspReport()
        {
            var filePath = Server.MapPath("~/Reports/CspReport.txt");
            
            if (System.IO.File.Exists(filePath))
            {
                return File(filePath, "text/plain", "CspReport.txt");
            }

            return HttpNotFound("Finnes ingen rapport ennå");
        }

        [HttpPost]
        public EmptyResult RegisterReport()
        {
            CspReportRequest reportRequest = ProcessCspValidationReport();

            AddReport(reportRequest);
                
            return new EmptyResult();
        }

        private CspReportRequest ProcessCspValidationReport()
        {
            Request.InputStream.Position = 0;
            using (StreamReader inputStream = new StreamReader(Request.InputStream))
            {
                string s = inputStream.ReadToEnd();
                if (!string.IsNullOrWhiteSpace(s))
                {
                    CspReportRequest cspPost = JsonConvert.DeserializeObject<CspReportRequest>(s);
                    return cspPost;
                }
            }

            return null;
        }


        private void AddReport(CspReportRequest reportRequest)
        {
            if (reportRequest != null && reportRequest.CspReport != null)
            {
                var mapPath = Server.MapPath("~/Reports/CspReport.txt");

                if (mapPath != null)
                {
                    try
                    {
                        System.IO.File.AppendAllText(mapPath, reportRequest.CspReport.ToString());
                    }
                    catch (IOException) { }
                }
            }
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
            SelfDataUrl  = Self | Data | Url,
            AllData  = All | Data
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
                sources.Add("*");
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