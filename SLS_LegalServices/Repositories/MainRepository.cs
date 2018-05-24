using SLS_LegalServices.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SLS_LegalServices.Repositories
{
    public class MainRepository: IMainRepository
    {
        SLS_LegalServicesEntities context = new SLS_LegalServicesEntities();

        
        #region CaseTypes
        public List<CaseTypesVM> GetAllCaseTypes()
        {
            List<CaseTypesVM> list = context.CaseTypes
                .Select(o => new CaseTypesVM { TypeId = o.TypeId, Active = o.Active, Description = o.Description })
                .OrderBy(o => o.TypeId)
                .ToList();
            return list;
        }

        public void CaseTypesDelete(CaseTypesVM vm)
        {
            try
            {
                CaseType newRecord = context.CaseTypes.Where(c => c.TypeId == vm.TypeId).FirstOrDefault();
                context.CaseTypes.Remove(newRecord);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CaseTypesInsert(CaseTypesVM vm)
        {
            CaseType newRecord = new CaseType { Active = vm.Active, Description = vm.Description };
            context.CaseTypes.Add(newRecord);
            context.SaveChanges();
        }

        public void CaseTypesUpdate(CaseTypesVM vm)
        {
            CaseType newRecord = context.CaseTypes.Where(c => c.TypeId == vm.TypeId).FirstOrDefault();
            newRecord.Active = vm.Active;
            newRecord.Description = vm.Description;
            context.SaveChanges();
        }

        public List<AttorneyModel> GetAllAttorneys()
        {
            throw new NotImplementedException();
        }

        public void AttorneyInsert(AttorneyModel vm)
        {
            throw new NotImplementedException();
        }

        public void AttorneyDelete(AttorneyModel vm)
        {
            throw new NotImplementedException();
        }

        public void AttorneyUpdate(AttorneyModel vm)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}