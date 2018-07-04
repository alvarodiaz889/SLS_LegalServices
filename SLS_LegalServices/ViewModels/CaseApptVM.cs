using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLS_LegalServices.ViewModels
{
    public class CaseApptVM : ISchedulerEvent
    {
        public string Title {
            get
            {
                var temp = CaseNo + "-" + InternFullName.Trim();
                temp = (temp.StartsWith("-") || temp.EndsWith("-")) ? temp.Replace("-", string.Empty) : temp;
                return temp;
            }
            set { }
        }
        public string Description { get; set; }
        public bool IsAllDay { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }


        public int CaseApptId { get; set; }
        public int CaseId { get; set; }
        public string CaseNo { get; set; }
        public int InternId { get; set; }
        public string InternFullName { get; set; }

    }
}