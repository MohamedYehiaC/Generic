using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.API.Helper
{
    public class AppSettings
    {
        public string AppBase { get; set; }
        public string WorkflowConnectServiceUrl { get; set; }
        public string WorkflowAdminConnectServiceUrl { get; set; }
        public string WFAppNames { get; set; }
        public string AuthorizedDomain { get; set; }
        public string AuthorizedUser { get; set; }
        public string AuthorizedCode { get; set; }

        public string SharedDirectoryPath { get; set; }
        public string RequestRelativePath { get; set; }
        public string VersionRelativePath { get; set; }
        public string AppID { get; set; }
        public string TrainingAndEvaluationProcessID { get; set; }
        public string SWRequestProcessID { get; set; }
        public string Locale { get; set; }
    }
}
