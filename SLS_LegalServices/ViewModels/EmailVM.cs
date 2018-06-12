using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLS_LegalServices.ViewModels
{
    public class EmailVM
    {
        public int EmailId { get; set; }
        public string Type { get; set; }
        public int? CaseId { get; set; }
        public string Email1 { get; set; }
        public CaseVM Case { get; set; }
    }
}