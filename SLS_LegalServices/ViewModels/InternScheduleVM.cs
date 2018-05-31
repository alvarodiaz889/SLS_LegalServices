using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SLS_LegalServices.ViewModels
{
    public class InternScheduleVM
    {
        public int InternScheduleID { get; set; }

        public int? InternId { get; set; }

        private string _strInternId;
        public string StrInternId {
            get {
                //if (InternId.HasValue)
                //    return InternId.Value.ToString();
                //else
                //    return string.Empty;
                return _strInternId;
            }
            set {
                _strInternId = value;
            }
        }

        [UIHint("DaysOfWeek")]
        public string DayOfWeek { get; set; }

        private DateTime? _startTime;
        [UIHint("StartTime")]
        public DateTime? StartTime {
            get { return _startTime; }
            set { _startTime = value; }
        }

        private DateTime? _endTime;
        [UIHint("EndTime")]
        public DateTime? EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }
    }
}