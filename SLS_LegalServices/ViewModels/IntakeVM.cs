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

        public string Status { get; set; }

        [Display(Name = "Created")]
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

        //Extra fields
        public string AdverseParty { get; set; }
        public string Intern { get; set; }
        public string Interview { get; set; }
        public string FullName {
            get {
                var full = string.Empty;
                full += string.IsNullOrEmpty(LastName) ? string.Empty : LastName;
                full += string.IsNullOrEmpty(LastName) ? string.Empty : ", " + FirstName;
                return full;
            }
        }

        [UIHint("CaseInternDDL")]
        [Display(Name = "Assigned Intern")]
        public int? InternId { get; set; }

        [UIHint("AttorneysMultiSelect")]
        public List<AttorneyVM> Attorneys { get; set; }
        public int[] AttorneyIds { get; set; }

        public List<ReferralSourceVM> ReferralSources { get; set; }

    }
}