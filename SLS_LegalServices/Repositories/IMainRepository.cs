using SLS_LegalServices.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLS_LegalServices.Repositories
{
    public interface IMainRepository
    {
        #region CaseTypes
        List<CaseTypesVM> GetAllCaseTypes();
        void CaseTypesInsert(CaseTypesVM vm);
        void CaseTypesDelete(CaseTypesVM vm);
        void CaseTypesUpdate(CaseTypesVM vm);
        #endregion

        #region Attorneys
        List<AttorneyVM> GetAllAttorneys();
        void AttorneyInsert(AttorneyVM vm);
        void AttorneyDelete(AttorneyVM vm);
        void AttorneyUpdate(AttorneyVM vm);
        #endregion

        #region CaseLog
        List<LogVM> GetAllCaseLogs();
        void CaseLogInsert(LogVM vm);
        void CaseLogDelete(LogVM vm);
        void CaseLogUpdate(LogVM vm);
        #endregion

        #region Interns
        List<InternVM> GetAllInterns();
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

        List<CaseVM> GetAllCases();
        void CaseInsert(CaseVM vm);
        void CaseDelete(CaseVM vm);
        void CaseUpdate(CaseVM vm);

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


        void Dispose();
    }
}
