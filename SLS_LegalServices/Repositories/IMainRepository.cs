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
        void IntakeInsert(IntakeVM vm);
        void IntakeDelete(IntakeVM vm);
        void IntakeUpdate(IntakeVM vm);

        #endregion

        #region Cases

        List<CaseVM> GetAllCases();
        void CaseInsert(CaseVM vm);
        void CaseDelete(CaseVM vm);
        void CaseUpdate(CaseVM vm);

        #endregion



        void Dispose();
    }
}
