using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLS_LegalServices.ViewModels
{
    public class LogVM
    {
        public int LogId { get; set; }
        public string LogType { get; set; }
        public int? CaseId { get; set; }
        public DateTime LogDate { get; set; }
        public Guid CreatedById { get; set; }
        public string Action { get; set; }
        public string Detail { get; set; }
        public int Active { get; set; }
        public string UserName { get; set; }
        public string CaseCode { get; set; }

    }
}