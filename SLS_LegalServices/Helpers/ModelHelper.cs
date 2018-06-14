using SLS_LegalServices.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLS_LegalServices.Helpers
{
    public class ModelHelper
    {

        public static Func<Case, IntakeVM> GetIntakeFromModelFunc = GetIntakeFromModel;
        public static IntakeVM GetIntakeFromModel(Case obj)
        {
            return new IntakeVM {
                CaseId = obj.CaseId,
                CaseNo = obj.CaseNo,
                TypeId = obj.TypeId,
                Status = obj.Status,
                CreationDate = obj.CreationDate,
                CreatedById = obj.CreatedById,
                MayWeRecordYourInterview = obj.MayWeRecordYourInterview,
                Narrative = obj.Narrative,
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                OrganizationName = obj.OrganizationName,
                Gender = obj.Gender,
                AcademicStatus = obj.AcademicStatus,
                SocialStatus = obj.SocialStatus,
                CitizenshipStatus = obj.CitizenshipStatus,
                IUStudentId = obj.IUStudentId,
                EnrollmentVerified = obj.EnrollmentVerified,
                StudentActivityFeeVerified = obj.StudentActivityFeeVerified
            };
        }

        public static Func<IntakeVM, Case> GetIntakeFromViewModelFunc = GetIntakeFromViewModel;
        public static Case GetIntakeFromViewModel(IntakeVM obj)
        {
            return new Case
            {
                CaseId = obj.CaseId,
                CaseNo = obj.CaseNo,
                TypeId = obj.TypeId,
                Status = obj.Status,
                CreationDate = obj.CreationDate,
                CreatedById = obj.CreatedById,
                MayWeRecordYourInterview = obj.MayWeRecordYourInterview,
                Narrative = obj.Narrative,
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                OrganizationName = obj.OrganizationName,
                Gender = obj.Gender,
                AcademicStatus = obj.AcademicStatus,
                SocialStatus = obj.SocialStatus,
                CitizenshipStatus = obj.CitizenshipStatus,
                IUStudentId = obj.IUStudentId,
                EnrollmentVerified = obj.EnrollmentVerified,
                StudentActivityFeeVerified = obj.StudentActivityFeeVerified
            };
        }

        public static Func<CaseType, CaseTypesVM> GetCaseTypeFromModelFunc = GetCaseTypeFromModel;
        public static CaseTypesVM GetCaseTypeFromModel(CaseType obj)
        {
            return new CaseTypesVM
            {
                Active = obj.Active,
                Description = obj.Description,
                TypeCode = obj.TypeCode,
                TypeId = obj.TypeId
            };
        }

        public static Func<CaseTypesVM, CaseType> GetCaseTypeFromViewModelFunc = GetCaseTypeFromViewModel;
        public static CaseType GetCaseTypeFromViewModel(CaseTypesVM obj)
        {
            return new CaseType
            {
                Active = obj.Active,
                Description = obj.Description,
                TypeCode = obj.TypeCode,
                TypeId = obj.TypeId
            };
        }

        public static Func<Attorney, AttorneyVM> GetAttorneyFromModelFunc = GetAttorneyFromModel;
        public static AttorneyVM GetAttorneyFromModel(Attorney obj)
        {
            return new AttorneyVM
            {
                AttorneyId = obj.AttorneyId,
                UserName = obj.User.UserName,
                DisplayName = obj.User.DisplayName,
                FirstName = obj.User.FirstName,
                LastName = obj.User.LastName,
                Active = obj.User.Active,
                UserId = obj.UserId
            };
        }

        public static Func<Address, AddressVM> GetAddressFromModelFunc = GetAddressFromModel;
        public static AddressVM GetAddressFromModel(Address obj)
        {
            return new AddressVM
            {
                Address1 = obj.Address1,
                Address2 = obj.Address2,
                AddressId = obj.AddressId,
                CaseId = obj.CaseId,
                City = obj.City,
                Country = obj.Country,
                State = obj.State,
                Type = obj.Type,
                PostalCode = obj.PostalCode
            };
        }

        public static Func<Email, EmailVM> GetEmailFromModelFunc = GetEmailFromModel;
        public static EmailVM GetEmailFromModel(Email obj)
        {
            return new EmailVM
            {
                CaseId = obj.CaseId,
                Email1 = obj.Email1,
                EmailId = obj.EmailId,
                Type = obj.Type
            };
        }

    }
}