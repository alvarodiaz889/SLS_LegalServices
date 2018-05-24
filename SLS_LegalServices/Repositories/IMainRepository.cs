using SLS_LegalServices.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLS_LegalServices.Repositories
{
    interface IMainRepository
    {
        #region CaseTypes
        List<CaseTypesVM> GetAllCaseTypes();
        void CaseTypesInsert(CaseTypesVM vm);
        void CaseTypesDelete(CaseTypesVM vm);
        void CaseTypesUpdate(CaseTypesVM vm);
        #endregion

        #region Attorneys
        List<AttorneyModel> GetAllAttorneys();
        void AttorneyInsert(AttorneyModel vm);
        void AttorneyDelete(AttorneyModel vm);
        void AttorneyUpdate(AttorneyModel vm);
        #endregion


    }
}
