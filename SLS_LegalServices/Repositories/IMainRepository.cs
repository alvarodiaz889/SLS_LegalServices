﻿using SLS_LegalServices.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLS_LegalServices.Repositories
{
    public interface IMainRepository
    {
        void SetLoggedUserId(Guid id);

        #region CaseTypes
        List<CaseTypesVM> GetAllCaseTypes();
        void CaseTypesInsert(CaseTypesVM vm);
        void CaseTypesDelete(CaseTypesVM vm);
        void CaseTypesUpdate(CaseTypesVM vm);
        #endregion

        #region Attorneys
        List<AttorneyVM> GetAllAttorneys();
        AttorneyVM GetAttorneyById(int id);
        void AttorneyInsert(AttorneyVM vm);
        void AttorneyDelete(AttorneyVM vm);
        void AttorneyUpdate(AttorneyVM vm);
        #endregion

        #region CaseLog
        List<LogVM> GetAllCaseLogs();
        List<LogVM> GetAllCaseLogsByCaseId(int caseId);
        void CaseLogInsert(LogVM vm);
        void CaseLogDelete(LogVM vm);
        void CaseLogUpdate(LogVM vm);
        void LogIntake_MainInfo(string action, IntakeVM old, IntakeVM recent);

        #endregion

        #region Interns
        List<InternVM> GetAllInterns();
        InternVM GetInternById(int id);
        void InternInsert(InternVM vm);
        void InternDelete(InternVM vm);
        void InternUpdate(InternVM vm);
        List<AttorneyVM> GetAllAttorneysByIntern(InternVM intern);
        #endregion

        #region InternSchedule
        List<InternScheduleVM> GetAllInternSchedules();
        void InternScheduleInsert(InternScheduleVM vm);
        void InternScheduleDelete(InternScheduleVM vm);
        void InternScheduleDeleteAllFromIntern(InternScheduleVM vm);
        void InternScheduleUpdate(InternScheduleVM vm);
        List<InternScheduleVM> GetAllScheduleByIntern(InternVM intern);
        #endregion

        #region Intakes
        List<IntakeVM> GetAllIntakes();
        IntakeVM GetIntakeById(int id);
        int IntakeInsert(IntakeVM vm);
        void IntakeDelete(IntakeVM vm);
        void IntakeUpdate(IntakeVM vm);
        #endregion

        #region Cases

        List<IntakeVM> GetAllCasesBy(Func<Case, bool> predicate);
        List<IntakeVM> GetAllCasesByStatus(string status);
        IntakeVM GetCaseById(int id);
        List<IntakeVM> GetCasesByAttorneyId(int attorneyId);
        void CaseInsert(int caseId, string caseNo);
        void CaseDelete(IntakeVM vm);
        void CaseUpdate(IntakeVM vm);
        #endregion

        #region Telephone
        List<TelephoneVM> GetAllTelephones();
        void TelephoneInsert(TelephoneVM vm);
        void TelephoneDelete(TelephoneVM vm);
        void TelephoneUpdate(TelephoneVM vm);
        #endregion

        #region Email
        List<EmailVM> GetAllEmails();
        int EmailInsert(EmailVM vm);
        void EmailDelete(EmailVM vm);
        void EmailUpdate(EmailVM vm);
        #endregion

        #region Address
        List<AddressVM> GetAllAddresses();
        int AddressInsert(AddressVM vm);
        void AddressDelete(AddressVM vm);
        void AddressUpdate(AddressVM vm);
        #endregion

        #region CaseNotes
        List<CaseNotesVM> GetAllCaseNotes();
        void CaseNoteInsert(CaseNotesVM vm);
        void CaseNoteDelete(CaseNotesVM vm);
        void CaseNoteUpdate(CaseNotesVM vm);
        #endregion

        #region CaseNotes
        List<CasePartyVM> GetAllCaseParties();
        void CasePartyInsert(CasePartyVM vm);
        void CasePartyDelete(CasePartyVM vm);
        void CasePartyUpdate(CasePartyVM vm);
        #endregion

        #region CaseDocuments
        List<CaseDocumentVM> GetAllCaseDocuments();
        void CaseDocumentInsert(CaseDocumentVM vm);
        void CaseDocumentDelete(CaseDocumentVM vm);
        void CaseDocumentUpdate(CaseDocumentVM vm);
        #endregion

        #region CaseMoney
        List<CaseMoneyVM> GetAllCaseMoneys();
        void CaseMoneyInsert(CaseMoneyVM vm);
        void CaseMoneyDelete(CaseMoneyVM vm);
        void CaseMoneyUpdate(CaseMoneyVM vm);
        #endregion

        #region ReferralSources
        List<ReferralSourceVM> GetAllReferralSources();
        ReferralSourceVM GetReferralSourcesById(int id);
        void ReferralSourcesUpdate(int caseId, int referralId);
        #endregion

        #region GenericValuesLookup
        List<GenericValuesLookupVM> GetAllGenericValuesByType(string type);

        #endregion

        #region Schedule

        List<CaseApptVM> GetAllCaseApptByDay(string date);

        List<InternScheduleVM> GetInternSchedulesByInternAndDay(string date);

        void CaseApptInsert(CaseApptVM obj);
        void CaseApptUpdate(CaseApptVM obj);
        void CaseApptDelete(CaseApptVM obj);

        #endregion


        void Dispose();
    }
}
