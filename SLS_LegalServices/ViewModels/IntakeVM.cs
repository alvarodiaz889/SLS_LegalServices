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

        [UIHint("CaseTypeDDL")]
        [Display(Name = "Type")]
        public int? TypeId { get; set; }

        
        [UIHint("CaseStatusDDL")]
        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Created")]
        public DateTime CreationDate { get; set; }

        public Guid CreatedById { get; set; }

        public bool MayWeRecordYourInterview { get; set; }

        public string Narrative { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string OrganizationName { get; set; }

        [UIHint("GendersDDL")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [UIHint("AcademicStatusesDDL")]
        [Display(Name = "Academic Status")]
        public string AcademicStatus { get; set; }

        [UIHint("SocialStatusDDL")]
        [Display(Name = "Social Status")]
        public string SocialStatus { get; set; }

        [UIHint("CitizenshipStatus")]
        [Display(Name = "Citizenship Status")]
        public string CitizenshipStatus { get; set; }

        public string IUStudentId { get; set; }

        public bool EnrollmentVerified { get; set; }

        public bool StudentActivityFeeVerified { get; set; }

        //Extra fields
        public string AdverseParty { get; set; }
        public string Intern { get; set; }
        public string Interview { get; set; }
        public string FullName {
            get {
                var full = LastName + ", " + FirstName;
                full = full.StartsWith(", ") || full.EndsWith (", ") ? full.Replace(", ",string.Empty) : full;
                return full;
            }
        }

        [UIHint("CaseInternDDL")]
        [Display(Name = "Assigned Intern")]
        [AdditionalMetadata("IsCertified", 0)]
        public int? InternId { get; set; }

        [UIHint("CaseInternDDL")]
        [Display(Name = "Certified Intern")]
        [AdditionalMetadata("IsCertified", 2)]
        public int? CertifiedInternId { get; set; }

        [UIHint("AttorneysMultiSelect")]
        [Display(Name = "Attorney(s)")]
        public int[] AttorneyIds { get; set; }

        public List<ReferralSourceVM> ReferralSources { get; set; }

    }
}