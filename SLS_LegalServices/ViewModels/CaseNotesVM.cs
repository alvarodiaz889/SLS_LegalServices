using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SLS_LegalServices.ViewModels
{
    public class CaseNotesVM
    {
        public int CaseNoteId { get; set; }
        public int CaseId { get; set; }

        [Display(Name="Date/Time")]
        public DateTime CreationDate { get; set; }
        public string Detail { get; set; }
        public Guid CreatedById { get; set; }
       
        [Display(Name = "Entered By")]
        public string UserName { get; set; }
    }
}