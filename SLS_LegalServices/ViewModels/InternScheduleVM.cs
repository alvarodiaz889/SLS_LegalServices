using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLS_LegalServices.ViewModels
{
    public class InternScheduleVM
    {
        public int InternScheduleID { get; set; }
        public int? InternId { get; set; }
        public string DayOfWeek { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}