using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SLS_LegalServices.ViewModels
{
    public class InternVM
    {
        public int InternId { get; set; }
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(50)]
		[Display(Name = "User ID")]
        public string UserName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [UIHint("Date")]
        [Display(Name ="Certified")]
        public DateTime? CertifiedDate { get; set; }
        public string Status { get; set; }

        public List<InternScheduleVM> Schedules { get; set; }

        [UIHint("InternAttorneyDDL")]
        public List<AttorneyVM> Attorneys { get; set; }

        public string FullName {
            get {
                return LastName + " " + FirstName;
            }
        }

        public string Color { get => "Blue"; }
    }
}