using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SLS_LegalServices.ViewModels
{
    public class TelephoneVM
    {
        public int TelephoneId { get; set; }

        [Required]
        public string Type { get; set; }

        public int? CaseId { get; set; }

        [Required]
        public string Number { get; set; }

        public CaseVM Case { get; set; }
    }
}