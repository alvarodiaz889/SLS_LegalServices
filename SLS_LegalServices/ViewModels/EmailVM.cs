using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SLS_LegalServices.ViewModels
{
    public class EmailVM
    {
        public int EmailId { get; set; }
        public string Type { get; set; }
        public int? CaseId { get; set; }

        [Display(Name ="Email Address")]
        [Required]
        public string Email1 { get; set; }
        public CaseVM Case { get; set; }
    }
}