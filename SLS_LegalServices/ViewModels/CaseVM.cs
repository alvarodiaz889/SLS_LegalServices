using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLS_LegalServices.ViewModels
{
    public class CaseVM
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

    }
}