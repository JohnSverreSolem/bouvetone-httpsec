using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace Web.Models
{
    public class CspReportRequest
    {
        [JsonProperty(PropertyName = "csp-report")]
        public CspReport CspReport { get; set; }
    }

    public class CspReport
    {
        [JsonProperty(PropertyName = "document-uri")]
        public string DocumentUri { get; set; }

        [JsonProperty(PropertyName = "referrer")]
        public string Referrer { get; set; }

        [JsonProperty(PropertyName = "violated-directive")]
        public string ViolatedDirective { get; set; }

        [JsonProperty(PropertyName = "effective-directive")]
        public string EffectiveDirective { get; set; }

        [JsonProperty(PropertyName = "original-policy")]
        public string OriginalPolicy { get; set; }

        [JsonProperty(PropertyName = "blocked-uri")]
        public string BlockedUri { get; set; }

        [JsonProperty(PropertyName = "status-code")]
        public int StatusCode { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(DateTime.Now.ToString("G"));
            sb.AppendFormat("Document uri: {0}", DocumentUri).AppendLine();
            sb.AppendFormat("Referrer: {0}", Referrer).AppendLine();
            sb.AppendFormat("Blocked uri: {0}", BlockedUri).AppendLine();
            sb.AppendFormat("Violated directive: {0}", ViolatedDirective).AppendLine();
            sb.AppendFormat("Effective directive: {0}", EffectiveDirective).AppendLine();
            sb.AppendFormat("Original policy: {0}", OriginalPolicy).AppendLine();
            sb.AppendFormat("Status code: {0}", StatusCode).AppendLine();
            sb.AppendLine();

            return sb.ToString();
        }
    }
}