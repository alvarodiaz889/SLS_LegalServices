using SLS_LegalServices.Helpers;
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
                UserId = Guid.NewGuid(),
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
                CaseId = o.CaseId,
                LogType = o.LogType,
                UserName = o.User.UserName
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
                CaseId = vm.CaseId,
                LogType = vm.LogType,
                CreatedById = vm.CreatedById
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
                CaseId = vm.CaseId,
                LogType = vm.LogType,
                CreatedById = vm.CreatedById
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
                UserId = o.UserId,
                InternId = o.InternId,
                UserName = o.User.UserName,
                FirstName = o.User.FirstName,
                LastName = o.User.LastName,
                Status = o.Status,
                CertifiedDate = o.CertifiedDate,
            }).ToList();
            interns.ForEach(o => {
                o.Attorneys = GetAllAttorneysByIntern(o);
                o.Schedules = GetAllScheduleByIntern(o);
            });
            return interns;
        }

        public void InternInsert(InternVM vm)
        {
            User user = new User
            {
                UserId = Guid.NewGuid(),
                UserName = vm.UserName,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
            };
            Intern intern = new Intern
            {
                User = user,
                CertifiedDate = vm.CertifiedDate,
                Status = vm.Status
            };
            context.Interns.Add(intern);
            context.SaveChanges();

            foreach (var attorney in vm.Attorneys)
            {
                Intern_Attorney ia = new Intern_Attorney
                {
                    InternId = intern.InternId,
                    AttorneyId = attorney.AttorneyId,
                    CreationDate = DateTime.Now
                };
                context.Intern_Attorney.Add(ia);
            }
            
            context.SaveChanges();
        }

        public void InternDelete(InternVM vm)
        {
            var intern = context.Interns
                .Where(a => a.InternId == vm.InternId)
                .FirstOrDefault();
            context.Users.Remove(intern.User);
            context.Interns.Remove(intern);
            context.SaveChanges();
        }

        public void InternUpdate(InternVM vm)
        {
            //User and Intern
            User user = new User
            {
                UserId = vm.UserId,
                UserName = vm.UserName,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
            };
            Intern intern = new Intern
            {
                InternId = vm.InternId,
                UserId = vm.UserId,
                User = user,
                CertifiedDate = vm.CertifiedDate,
                Status = vm.Status
            };
            context.Users.Attach(user);
            context.Entry(user).State = EntityState.Modified;
            context.Interns.Attach(intern);
            context.Entry(intern).State = EntityState.Modified;
            context.SaveChanges();
            //Intern-Attorney
            context.Intern_Attorney.RemoveRange(intern.Intern_Attorney);
            context.SaveChanges();

            foreach (var attorney in vm.Attorneys)
            {
                Intern_Attorney ia = new Intern_Attorney
                {
                    InternId = intern.InternId,
                    AttorneyId = attorney.AttorneyId,
                    CreationDate = DateTime.Now
                };
                context.Intern_Attorney.Add(ia);
            }

            context.SaveChanges();
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

        #region InternSchedule
        public List<InternScheduleVM> GetAllInternSchedules()
        {
            return context.InternSchedules
                .Select(s => new InternScheduleVM() {
                    DayOfWeek = s.DayOfWeek,
                    EndTime = s.EndTime,
                    InternId = s.InternId,
                    InternScheduleID = s.InternScheduleID,
                    StartTime = s.StartTime
                }).ToList();
        }

        public void InternScheduleInsert(InternScheduleVM vm)
        {
            InternSchedule intern = new InternSchedule
            {
                InternId =vm.InternId,
                DayOfWeek = vm.DayOfWeek,
                EndTime = vm.EndTime,
                InternScheduleID = vm.InternScheduleID,
                StartTime = vm.StartTime
            };
            context.InternSchedules.Add(intern);
            context.SaveChanges();
        }

        public void InternScheduleDeleteAllFromIntern(InternScheduleVM vm)
        {
            List<InternSchedule> internSchedule = context.InternSchedules
                .Where(s => s.InternId == vm.InternId).ToList();
            context.InternSchedules.RemoveRange(internSchedule);
            context.SaveChanges();
        }

        public void InternScheduleDelete(InternScheduleVM vm)
        {
            InternSchedule internSchedule = context.InternSchedules
                .Where(s => s.InternScheduleID == s.InternScheduleID).FirstOrDefault();
            context.InternSchedules.Remove(internSchedule);
            context.SaveChanges();
        }

        public void InternScheduleUpdate(InternScheduleVM vm)
        {
            InternSchedule internSchedule = new InternSchedule
            {
                DayOfWeek = vm.DayOfWeek,
                EndTime = vm.EndTime,
                InternId = vm.InternId,
                InternScheduleID = vm.InternScheduleID,
                StartTime = vm.StartTime
            };
            context.InternSchedules.Attach(internSchedule);
            context.Entry(internSchedule).State = EntityState.Modified;
            context.SaveChanges();
        }

        public List<InternScheduleVM> GetAllScheduleByIntern(InternVM intern)
        {
            return context.InternSchedules
                .Where(s => s.InternId == intern.InternId)
                .Select(s => new InternScheduleVM()
                {
                    DayOfWeek = s.DayOfWeek,
                    EndTime = s.EndTime,
                    InternId = s.InternId,
                    InternScheduleID = s.InternScheduleID,
                    StartTime = s.StartTime
                }).ToList();
        }


        #endregion

        #region Intakes

        public List<IntakeVM> GetAllIntakes()
        {
            return context.Cases
                .Where(c => c.CaseNo == null)
                .Select(ModelHelper.GetIntakeFromModelFunc)
                .ToList();
        }

        public IntakeVM GetIntakeById(int id)
        {
            return context.Cases
                .Where(c => c.CaseId == id)
                .Select(ModelHelper.GetIntakeFromModelFunc)
                .FirstOrDefault();
        }

        public int IntakeInsert(IntakeVM vm)
        {
            int insertedId = 0;
            int rowsAfected = 0;
            Case caseObj = ModelHelper.GetIntakeFromViewModel(vm);
            caseObj.Status = "Pending";
            caseObj.CreationDate = DateTime.Now;

            context.Cases.Add(caseObj);
            rowsAfected = context.SaveChanges();
            insertedId = caseObj.CaseId;
            if (rowsAfected > 0 && vm.CaseInternId.HasValue)
            {
                CaseIntern ci = new CaseIntern
                {
                    CaseId = caseObj.CaseId,
                    InternId = vm.CaseInternId.Value,
                    CreationDate = DateTime.Now
                };
                context.CaseInterns.Add(ci);
                context.SaveChanges();
            }
            return insertedId;
        }

        public void IntakeDelete(IntakeVM vm)
        {
            Case obj = context.Cases.Where(c => c.CaseId == vm.CaseId).FirstOrDefault();
            context.Cases.Remove(obj);
            context.SaveChanges();
        }

        public void IntakeUpdate(IntakeVM vm)
        {
            Case obj = context.Cases.Where(c => c.CaseId == vm.CaseId).FirstOrDefault();
            obj.FirstName = vm.FirstName;
            obj.LastName = vm.LastName;
            obj.IUStudentId = vm.IUStudentId;
            obj.TypeId = vm.TypeId;

            context.Cases.Attach(obj);
            context.Entry(obj).State = EntityState.Modified;
            context.SaveChanges();
            if (vm.CaseInternId.HasValue)
            {
                List<CaseIntern> caseInternList = context.CaseInterns
                    .Where(c => c.InternId == vm.CaseInternId.Value).ToList();
                context.CaseInterns.RemoveRange(caseInternList);
                CaseIntern ci = new CaseIntern
                {
                    CaseId = obj.CaseId,
                    InternId = vm.CaseInternId.Value,
                    CreationDate = DateTime.Now
                };
                context.CaseInterns.Add(ci);
                context.SaveChanges();
            }

        }

        public List<CaseVM> GetAllCases()
        {
            throw new NotImplementedException();
        }

        public void CaseInsert(CaseVM vm)
        {
            throw new NotImplementedException();
        }

        public void CaseDelete(CaseVM vm)
        {
            throw new NotImplementedException();
        }

        public void CaseUpdate(CaseVM vm)
        {
            throw new NotImplementedException();
        }

        #endregion


    }
}