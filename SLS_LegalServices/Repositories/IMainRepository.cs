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

        void Dispose();
    }
}
