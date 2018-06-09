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
    }
}