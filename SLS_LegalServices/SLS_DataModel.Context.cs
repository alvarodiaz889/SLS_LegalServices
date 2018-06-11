﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SLS_LegalServicesEntities : DbContext
    {
        public SLS_LegalServicesEntities()
            : base("name=SLS_LegalServicesEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Attorney> Attorneys { get; set; }
        public virtual DbSet<Intern_Attorney> Intern_Attorney { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<InternSchedule> InternSchedules { get; set; }
        public virtual DbSet<Intern> Interns { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<CaseAttorney> CaseAttorneys { get; set; }
        public virtual DbSet<CaseDocument> CaseDocuments { get; set; }
        public virtual DbSet<CaseIntern> CaseInterns { get; set; }
        public virtual DbSet<CaseMoney> CaseMoneys { get; set; }
        public virtual DbSet<CaseNote> CaseNotes { get; set; }
        public virtual DbSet<CaseReferralSource> CaseReferralSources { get; set; }
        public virtual DbSet<CaseType> CaseTypes { get; set; }
        public virtual DbSet<CaseCertifiedIntern> CaseCertifiedInterns { get; set; }
        public virtual DbSet<CaseStatusLookup> CaseStatusLookups { get; set; }
        public virtual DbSet<GenericValuesLookup> GenericValuesLookups { get; set; }
        public virtual DbSet<CaseParty> CaseParties { get; set; }
        public virtual DbSet<Case> Cases { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Email> Emails { get; set; }
        public virtual DbSet<Telephone> Telephones { get; set; }
    }
}
