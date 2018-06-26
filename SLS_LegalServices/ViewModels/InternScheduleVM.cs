using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SLS_LegalServices.ViewModels
{
    public class InternScheduleVM
    {
        public int InternScheduleID { get; set; }

        public int? InternId { get; set; }

        private string _strInternId;
        public string StrInternId {
            get {
                return _strInternId;
            }
            set {
                _strInternId = value;
            }
        }

        [UIHint("DaysOfWeek")]
        [Required]
        public string DayOfWeek { get; set; }

        [UIHint("TimePicker")]
        [AdditionalMetadata("Name", "StartTime")]
        [Required]
        public DateTime? StartTime { get; set; }

        [UIHint("TimePicker")]
        [AdditionalMetadata("Name", "EndTime")]
        [Required]
        public DateTime? EndTime { get; set; }
    }
}