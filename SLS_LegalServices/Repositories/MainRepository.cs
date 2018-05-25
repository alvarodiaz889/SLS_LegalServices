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
        IUserRepository userRepository = new UserRepositoryImpl();
        
        #region CaseTypes
        public List<CaseTypesVM> GetAllCaseTypes()
        {
            List<CaseTypesVM> list = context.CaseTypes
                .Select(o => new CaseTypesVM { TypeId = o.TypeId, Active = o.Active, Description = o.Description, TypeCode = o.TypeCode })
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
            CaseType newRecord = new CaseType { Active = vm.Active, Description = vm.Description, TypeCode = vm.TypeCode };
            context.CaseTypes.Add(newRecord);
            context.SaveChanges();
        }

        public void CaseTypesUpdate(CaseTypesVM vm)
        {
            CaseType newRecord = context.CaseTypes.Where(c => c.TypeId == vm.TypeId).FirstOrDefault();
            newRecord.Active = vm.Active;
            newRecord.Description = vm.Description;
            newRecord.TypeCode = vm.TypeCode;
            context.SaveChanges();
        }
        #endregion

        #region Attorneys
        public List<AttorneyVM> GetAllAttorneys()
        {
            return context.Attorneys.Select(s => new AttorneyVM {
                AttorneyId = s.AttorneyId,
                UserName = s.User.UserName,
                DisplayName = s.User.DisplayName,
                FirstName = s.User.FirstName,
                LastName = s.User.LastName,
                Active = s.User.Active,
                UserId = s.UserId
            }).ToList();
        }

        public void AttorneyInsert(AttorneyVM vm)
        {
            User user = new User {
                UserId = vm.UserId,
                Active = vm.Active,
                UserName = vm.UserName,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                DisplayName = vm.DisplayName,
            };
            Attorney attorney = new Attorney() { User = user };
            context.Attorneys.Add(attorney);
            context.SaveChanges();
        }

        public void AttorneyDelete(AttorneyVM vm)
        {
            Attorney attorney = context.Attorneys
                .Include(a => a.User)
                .Where(a => a.AttorneyId == vm.AttorneyId)
                .FirstOrDefault();
            context.Users.Remove(attorney.User);
            context.Attorneys.Remove(attorney);
            context.SaveChanges();
        }

        public void AttorneyUpdate(AttorneyVM vm)
        {
            User user = new User {
                Active = vm.Active,
                UserName = vm.UserName,
                LastName = vm.LastName,
                FirstName = vm.FirstName,
                DisplayName = vm.DisplayName,
                UserId = vm.UserId,
            };
            context.Users.Attach(user);
            context.Entry(user).State = EntityState.Modified;
            context.SaveChanges();
        }
        #endregion

        #region CaseLogs
        public List<LogVM> GetAllCaseLogs()
        {
            return context.Logs.Select(o => new LogVM()
            {
                Action = o.Action,
                Active = o.Active,
                Detail = o.Detail,
                LogDate = o.LogDate,
                LogId = o.LogId,
                LogParentId = o.LogParentId,
                LogType = o.LogType,
                UserName = o.UserName
            }).ToList();
        }

        public void CaseLogInsert(LogVM vm)
        {
            Log log = new Log
            {
                Action = vm.Action,
                Active = vm.Active,
                Detail = vm.Detail,
                LogDate = vm.LogDate,
                LogId = vm.LogId,
                LogParentId = vm.LogParentId,
                LogType = vm.LogType,
                UserName = vm.UserName
            };
            context.Logs.Add(log);
            context.SaveChanges();
        }

        public void CaseLogDelete(LogVM vm)
        {
            Log log = context.Logs.Where(o => o.LogId == vm.LogId).FirstOrDefault();
            context.Logs.Remove(log);
            context.SaveChanges();
        }

        public void CaseLogUpdate(LogVM vm)
        {
            Log log = new Log
            {
                Action = vm.Action,
                Active = vm.Active,
                Detail = vm.Detail,
                LogDate = vm.LogDate,
                LogId = vm.LogId,
                LogParentId = vm.LogParentId,
                LogType = vm.LogType,
                UserName = vm.UserName
            };
            context.Logs.Attach(log);
            context.Entry(log).State = EntityState.Modified;
            context.SaveChanges();
        }
        #endregion

        #region Interns
        public List<InternVM> GetAllInterns()
        {
            List<InternVM> interns =  context.Interns.Select(o => new InternVM() {
                InternId = o.InternId,
                UserName = o.User.UserName,
                FirstName = o.User.FirstName,
                LastName = o.User.LastName,
                Status = o.Status,
                CertifiedDate = o.CertifiedDate,
            }).ToList();
            interns.ForEach(o => o.Attorneys = GetAllAttorneysByIntern(o));
            return interns;
        }

        public void InternInsert(InternVM vm)
        {
            throw new NotImplementedException();
        }

        public void InternDelete(InternVM vm)
        {
            throw new NotImplementedException();
        }

        public void InternUpdate(InternVM vm)
        {
            throw new NotImplementedException();
        }

        public List<AttorneyVM> GetAllAttorneysByIntern(InternVM intern)
        {
            return context.Intern_Attorney.Where(o => o.InternId == intern.InternId)
                .Select(o => new AttorneyVM() {
                    AttorneyId = o.AttorneyId,
                    UserName = o.Attorney.User.UserName,
                    DisplayName = o.Attorney.User.DisplayName,
                    FirstName = o.Attorney.User.FirstName,
                    LastName = o.Attorney.User.LastName,
                    Active = o.Attorney.User.Active,
                    UserId = o.Attorney.UserId
                }).ToList();
        }
        #endregion
        public void Dispose()
        {
            context.Dispose();
        }
    }
}