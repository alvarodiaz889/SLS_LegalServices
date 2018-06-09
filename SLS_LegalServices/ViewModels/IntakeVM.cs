using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SLS_LegalServices.ViewModels
{
    public class IntakeVM
    {
        public int CaseId { get; set; }
        public string CaseNo { get; set; }
        public int TypeId { get; set; }
        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid CreatedById { get; set; }
        public bool MayWeRecordYourInterview { get; set; }
        public string Narrative { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OrganizationName { get; set; }
        public string Gender { get; set; }
        public string AcademicStatus { get; set; }
        public string SocialStatus { get; set; }
        public string CitizenshipStatus { get; set; }
        public string IUStudentId { get; set; }
        public bool EnrollmentVerified { get; set; }
        public bool StudentActivityFeeVerified { get; set; }
        public string AdverseParty { get; set; }
        public string Intern { get; set; }
        public string Interview { get; set; }

        [UIHint("DDLookup")]
        [Display(Name = "Type")]
        [AdditionalMetadata("Name", "CaseType")]
        [AdditionalMetadata("DataTextField", "Description")]
        [AdditionalMetadata("DataValueField", "TypeId")]
        [AdditionalMetadata("Controller", "Intakes")]
        [AdditionalMetadata("Action", "GetTypes")]
        public CaseType CaseType { get; set; }

        [UIHint("DDLookup")]
        [Display(Name = "Assigned Intern")]
        [AdditionalMetadata("Name", "CaseIntern")]
        [AdditionalMetadata("DataTextField", "LastName")]
        [AdditionalMetadata("DataValueField", "InternId")]
        [AdditionalMetadata("Controller", "Intakes")]
        [AdditionalMetadata("Action", "GetInterns")]
        public InternVM CaseIntern { get; set; }

    }
}