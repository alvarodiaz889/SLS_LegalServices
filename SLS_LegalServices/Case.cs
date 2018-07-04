//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SLS_LegalServices
{
    using System;
    using System.Collections.Generic;
    
    public partial class Case
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Case()
        {
            this.Addresses = new HashSet<Address>();
            this.CaseAttorneys = new HashSet<CaseAttorney>();
            this.CaseCertifiedInterns = new HashSet<CaseCertifiedIntern>();
            this.CaseInterns = new HashSet<CaseIntern>();
            this.CaseMoneys = new HashSet<CaseMoney>();
            this.CaseNotes = new HashSet<CaseNote>();
            this.CaseParties = new HashSet<CaseParty>();
            this.Emails = new HashSet<Email>();
            this.Telephones = new HashSet<Telephone>();
            this.CaseDocuments = new HashSet<CaseDocument>();
            this.ReferralSources = new HashSet<ReferralSource>();
            this.Logs = new HashSet<Log>();
            this.CaseAppts = new HashSet<CaseAppt>();
        }
    
        public int CaseId { get; set; }
        public string CaseNo { get; set; }
        public Nullable<int> TypeId { get; set; }
        public string Status { get; set; }
        public System.DateTime CreationDate { get; set; }
        public System.Guid CreatedById { get; set; }
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
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Address> Addresses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CaseAttorney> CaseAttorneys { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CaseCertifiedIntern> CaseCertifiedInterns { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CaseIntern> CaseInterns { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CaseMoney> CaseMoneys { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CaseNote> CaseNotes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CaseParty> CaseParties { get; set; }
        public virtual CaseType CaseType { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Email> Emails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Telephone> Telephones { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CaseDocument> CaseDocuments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReferralSource> ReferralSources { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Log> Logs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CaseAppt> CaseAppts { get; set; }
    }
}
