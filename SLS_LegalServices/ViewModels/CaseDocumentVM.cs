using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace SLS_LegalServices.ViewModels
{
    public class CaseDocumentVM
    {
        public int CaseDocumentId { get; set; }
        public int CaseId { get; set; }
        public string Filename { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid CreatedById { get; set; }
        public string FileType { get; set; }
        

        public string CreatedBy { get; set; }

        [UIHint("YesNoDropDown")]
        [AdditionalMetadata("Name", "IsWorldox")]
        public bool IsWorldox { get; set; }

        [UIHint("FileUpload")]
        public string ResumeFileUrl { get; set; }

    }
}