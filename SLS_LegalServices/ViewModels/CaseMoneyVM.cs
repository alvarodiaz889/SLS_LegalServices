using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLS_LegalServices.ViewModels
{
    public class CaseMoneyVM
    {
        public int CaseMoneyId { get; set; }
        public int CaseId { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid CreatedById { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string CreatedBy { get; set; }
    }
}