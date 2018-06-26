using SLS_LegalServices.Helpers;
using SLS_LegalServices.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;

namespace SLS_LegalServices.Repositories
{
    public class MainRepository: IMainRepository
    {
        SLS_LegalServicesEntities context = new SLS_LegalServicesEntities();
        private readonly IUserRepository userRepository = new UserRepositoryImpl();
        private readonly IRoleRepository roleRepository = new RoleRepositoryImpl();
        private Guid _user;

        public void SetLoggedUserId(Guid id)
        {
            _user = id;
        }

        #region CaseTypes
        public List<CaseTypesVM> GetAllCaseTypes()
        {
            List<CaseTypesVM> list = context.CaseTypes
                .Select(ModelHelper.GetCaseTypeFromModel)
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
            CaseType newRecord = ModelHelper.GetCaseTypeFromViewModel(vm);
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
            return context.Attorneys.Select(ModelHelper.GetAttorneyFromModel).ToList();
        }

        public AttorneyVM GetAttorneyById(int id)
        {
            return context.Attorneys.Where(a => a.AttorneyId == id).Select(ModelHelper.GetAttorneyFromModel).FirstOrDefault();
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

            userRepository.InsertApplicationUser(user.UserId.ToString(), user.UserName);
            roleRepository.Insert(user.UserId, "ATTORNEY");
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

            userRepository.DeleteApplicationUser(attorney.UserId.ToString());
            roleRepository.DeleteRoleByUserName(attorney.UserId.ToString());
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

            userRepository.UpdateApplicationUser(vm.UserId.ToString(),vm.UserName);
        }
        #endregion

        #region CaseLogs
        public List<LogVM> GetAllCaseLogs()
        {
            return context.Logs.Where(w => w.Case.CaseNo != null)
                .Select(o => new LogVM()
                {
                    Action = o.Action,
                    Active = o.Active,
                    Detail = o.Detail,
                    LogDate = o.LogDate,
                    LogId = o.LogId,
                    CaseId = o.CaseId,
                    LogType = o.LogType,
                    UserName = o.User.UserName,
                    CaseCode = o.Case.CaseNo
                })
                .OrderByDescending(o => o.LogDate)
                .ToList();
        }

        public List<LogVM> GetAllCaseLogsByCaseId(int caseId)
        {
            return context.Logs
                .Where(o => o.CaseId == caseId)
                .Select(o => new LogVM()
                {
                    Action = o.Action,
                    Active = o.Active,
                    Detail = o.Detail,
                    LogDate = o.LogDate,
                    LogId = o.LogId,
                    CaseId = o.CaseId,
                    LogType = o.LogType,
                    UserName = o.User.UserName,
                    CaseCode = o.Case.CaseNo
                })
                .OrderByDescending(o => o.LogDate)
                .ToList();
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

        public void LogIntake_MainInfo(string action, IntakeVM old, IntakeVM recent)
        {
            LogVM log;
            string logType = (old.CaseNo == null) ? "intakes" : "cases";

            switch (action)
            {
                case "Viewed":
                case "Created":
                    log = new LogVM
                    {
                        Action = action,
                        Active = 1,
                        CaseId = recent.CaseId,
                        CreatedById = recent.CreatedById,
                        LogDate = DateTime.Now,
                        LogType = logType,
                        Detail = ""
                    };
                    CaseLogInsert(log);
                    break;

                case "Updated":

                    StringBuilder detail = new StringBuilder("");

                    if (old.FirstName != recent.FirstName) detail.AppendLine("First Name Changed. - " + (recent.FirstName ?? string.Empty));
                    if (old.LastName != recent.LastName) detail.AppendLine("Last Name Changed. - " + (recent.LastName ?? string.Empty));
                    if (old.IUStudentId != recent.IUStudentId) detail.AppendLine("IU Student Id Changed. - " + (recent.IUStudentId ?? string.Empty));
                    if (old.TypeId != recent.TypeId) detail.AppendLine("Type Changed.");
                    if (old.Narrative != recent.Narrative) detail.AppendLine("Narrative Changed.");

                    if (old.InternId != recent.InternId)
                    {
                        //in case the record one record has 0 and the other null. Doesn't have to log it
                        var InternIdOne = old.InternId.HasValue && old.InternId != 0;
                        var InternIdTwo = recent.InternId.HasValue && recent.InternId != 0;
                        if (InternIdOne || InternIdTwo)
                        {
                            var text = string.Empty;
                            if (InternIdTwo) text = "Intern Changed. - " + GetAllInternById(recent.InternId.Value).FullName;
                            else text = "Intern Removed.";
                            detail.AppendLine(text);
                        } 
                    }
                    //validating attorney and gettin the names of the ones removed or added
                    var AttorneysOne = old.AttorneyIds != null && old.AttorneyIds.Length > 0;
                    var AttorneysTwo = recent.AttorneyIds != null && recent.AttorneyIds.Length > 0;
                    if (AttorneysOne && AttorneysTwo)
                    {
                        int[] greater;
                        int[] less;
                        string operation;
                        if (old.AttorneyIds.Length > recent.AttorneyIds.Length)
                        {
                            operation = "Removed";
                            greater = old.AttorneyIds;
                            less = recent.AttorneyIds;
                        }
                        else
                        {
                            operation = "Added";
                            greater = recent.AttorneyIds;
                            less = old.AttorneyIds;
                        }
                        //Got different Ids
                        var ids = greater.Where(w => Array.IndexOf(less, w) == -1).ToArray();
                        if (ids.Length > 0)
                        {
                            var names = context.Attorneys.Where(w => ids.Any(u => u == w.AttorneyId))
                            .Select(ModelHelper.GetAttorneyFromModel).Select(w => w.DisplayName).ToArray();

                            operation = "Attorney(s) '" + string.Join(", ", names) + "' " + operation;
                            detail.AppendLine(operation);
                        }
                        
                    }
                    else if (AttorneysOne)
                    {
                        var operation = "Removed";
                        var names = context.Attorneys.Where(w => old.AttorneyIds.Any(u => u == w.AttorneyId))
                            .Select(ModelHelper.GetAttorneyFromModel).Select(w => w.DisplayName).ToArray();

                        operation = "Attorney(s) '" + string.Join(", ", names) + "' " + operation;
                        detail.AppendLine(operation);
                    }
                    else if (AttorneysTwo)
                    {
                        var operation = "Added";
                        var names = context.Attorneys.Where(w => recent.AttorneyIds.Any(u => u == w.AttorneyId))
                            .Select(ModelHelper.GetAttorneyFromModel).Select(w => w.DisplayName).ToArray();

                        operation = "Attorney(s) '" + string.Join(", ", names) + "' " + operation;
                        detail.AppendLine(operation);
                    }

                    if (logType == "cases")
                    {
                        //Additional validations for cases
                        if (old.CertifiedInternId != recent.CertifiedInternId)
                        {
                            //in case the record one record has 0 and the other null. Doesn't have to log it
                            var InternIdOne = old.CertifiedInternId.HasValue && old.CertifiedInternId != 0;
                            var InternIdTwo = recent.CertifiedInternId.HasValue && recent.CertifiedInternId != 0;
                            if (InternIdOne || InternIdTwo)
                            {
                                var text = string.Empty;
                                if (InternIdTwo) text = "Certified Intern Changed. - " + GetAllInternById(recent.CertifiedInternId.Value).FullName;
                                else text = "Certified Intern Removed.";
                                detail.AppendLine(text);
                            }
                        }
                        if (old.CaseNo != recent.CaseNo) detail.AppendLine("Case Number Changed. - " + recent.CaseNo);
                        if (old.Status != recent.Status) detail.AppendLine("Status Changed. - " + recent.Status);
                        if (old.Gender != recent.Gender) detail.AppendLine("Gender Changed. - " + recent.Gender);
                        if (old.AcademicStatus != recent.AcademicStatus) detail.AppendLine("Academic Status Changed. - " + recent.AcademicStatus);
                        if (old.SocialStatus != recent.SocialStatus) detail.AppendLine("Social Status Changed. - " + recent.SocialStatus);
                        if (old.CitizenshipStatus != recent.CitizenshipStatus) detail.AppendLine("Citizenship Status Changed. - " + recent.CitizenshipStatus);
                    }

                    log = new LogVM
                    {
                        Action = action,
                        Active = 1,
                        CaseId = recent.CaseId,
                        CreatedById = old.CreatedById,
                        LogDate = DateTime.Now,
                        LogType = logType,
                        Detail = detail.ToString()
                    };
                    CaseLogInsert(log);
                    break;
            }
        }

        private void LogIntake_OtherInfo(string detail, int? caseId, Guid createdById)
        {
            string logType = caseId == null ? "intakes" : "cases";
            LogVM log = new LogVM
            {
                Action = "Updated",
                Active = 1,
                CaseId = caseId,
                CreatedById = createdById,
                LogDate = DateTime.Now,
                LogType = logType,
                Detail = detail
            };
            CaseLogInsert(log);
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

        public InternVM GetAllInternById(int id)
        {
            return context.Interns.Where(c => c.InternId == id)
                .Select(o => new InternVM()
                {
                    UserId = o.UserId,
                    InternId = o.InternId,
                    UserName = o.User.UserName,
                    FirstName = o.User.FirstName,
                    LastName = o.User.LastName,
                    Status = o.Status,
                    CertifiedDate = o.CertifiedDate,
                }).FirstOrDefault();
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

            userRepository.InsertApplicationUser(user.UserId.ToString(), user.UserName);
            roleRepository.Insert(user.UserId, "INTERN");
        }

        public void InternDelete(InternVM vm)
        {
            var intern = context.Interns
                .Where(a => a.InternId == vm.InternId)
                .FirstOrDefault();
            context.Users.Remove(intern.User);
            context.Interns.Remove(intern);
            context.SaveChanges();

            userRepository.DeleteApplicationUser(intern.UserId.ToString());
            roleRepository.DeleteRoleByUserName(intern.UserId.ToString());
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
            var interAttorneys = context.Intern_Attorney.Where(ia => ia.InternId == intern.InternId).ToList();
            context.Intern_Attorney.RemoveRange(interAttorneys);
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

            userRepository.UpdateApplicationUser(vm.UserId.ToString(), vm.UserName);
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
                .Select(c => new IntakeVM
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    CaseId = c.CaseId,
                    AdversePartyList = c.CaseParties.Where(cp => cp.CaseId == c.CaseId).Select(cp => (cp.FirstName + " " + cp.LastName)).ToList(),
                    InternFullName = context.CaseInterns.Where(cint => cint.CaseId == c.CaseId)
                        .Select(cint => (cint.Intern.User.FirstName + " " + cint.Intern.User.LastName)).FirstOrDefault(),
                    Type = (c.CaseType.TypeCode + "-" + c.CaseType.Description),
                    CreationDate = c.CreationDate
                }).ToList();
        }

        public IntakeVM GetIntakeById(int id)
        {
            IntakeVM vm =  context.Cases
                .Where(c => c.CaseId == id)
                .Select(ModelHelper.GetIntakeFromModelFunc)
                .FirstOrDefault();

            if (vm != null)
            {
                vm.InternId = context.CaseInterns.Where(c => c.CaseId == vm.CaseId)
                    .OrderByDescending(c => c.CreationDate)
                    .Select(c => c.InternId)
                    .FirstOrDefault();

                vm.CertifiedInternId = context.CaseCertifiedInterns.Where(c => c.CaseId == vm.CaseId)
                    .OrderByDescending(c => c.CreationDate)
                    .Select(c => c.InternId)
                    .FirstOrDefault();

                vm.AttorneyIds = context.Attorneys
                    .Where(a => a.CaseAttorneys.Any(ca => ca.CaseId == vm.CaseId))
                    .Select(a => a.AttorneyId)
                    .ToArray();

                vm.ReferralSources = context.ReferralSources.Where(r => r.Cases.Any(c => c.CaseId == vm.CaseId))
                        .Select(r => new ReferralSourceVM
                        {
                            ReferralSource1 = r.ReferralSource1,
                            ReferralSourceId = r.ReferralSourceId
                        }).ToList();
            }
            return vm;
        }

        public int IntakeInsert(IntakeVM vm)
        {
            int insertedId = 0;
            int rowsAfected = 0;

            Case caseObj = ModelHelper.GetIntakeFromViewModel(vm);
            caseObj.Status = vm.Status;
            caseObj.CreationDate = DateTime.Now;

            context.Cases.Add(caseObj);
            rowsAfected = context.SaveChanges();
            insertedId = caseObj.CaseId;


            if (rowsAfected > 0)
            {
                vm.CaseId = insertedId;
                IntakeInsertUpdate(true, vm);
            }

            return insertedId;
        }

        public void IntakeUpdate(IntakeVM vm)
        {
            IntakeInsertUpdate(false, vm);
        }

        private void IntakeInsertUpdate(bool insert, IntakeVM vm)
        {
            var oldVM = GetIntakeById(vm.CaseId);
            Case obj = context.Cases.Where(c => c.CaseId == vm.CaseId).FirstOrDefault();
            obj.FirstName = vm.FirstName;
            obj.LastName = vm.LastName;
            obj.IUStudentId = vm.IUStudentId;
            obj.TypeId = vm.TypeId;
            obj.Narrative = vm.Narrative;
            obj.Status = vm.Status;
            obj.Gender = vm.Gender;
            obj.AcademicStatus = vm.AcademicStatus;
            obj.SocialStatus = vm.SocialStatus;
            obj.CitizenshipStatus = vm.CitizenshipStatus;
            obj.CaseNo = vm.CaseNo;

            context.Cases.Attach(obj);
            context.Entry(obj).State = EntityState.Modified;
            context.SaveChanges();

            if (vm.InternId.HasValue)
            {
                ValidateIntakeIntern(oldVM, vm);
            }

            if (vm.CertifiedInternId.HasValue)
            {
                ValidateIntakeCertifiedIntern(oldVM, vm);
            }

            if (vm.AttorneyIds != null)
            {
                ValidateIntakeAttorneys(vm);
            }
            else if (vm.AttorneyIds == null && oldVM.AttorneyIds != null)
            {
                RemoveAllAttorneysFromIntakesVM(vm);
            }

            var action = insert ? "Created" : "Updated"; 
            LogIntake_MainInfo(action, oldVM, vm);
        }

        private void ValidateIntakeIntern(IntakeVM oldVM, IntakeVM vm)
        {
            List<CaseIntern> caseInternList = context.CaseInterns
                    .Where(c => c.InternId == vm.InternId.Value)
                    .Where(c => c.CaseId == vm.CaseId)
                    .OrderByDescending(c => c.CreationDate)
                    .ToList();
            if (caseInternList.Count == 0)
            {
                CaseIntern ci = new CaseIntern
                {
                    CaseId = vm.CaseId,
                    InternId = vm.InternId.Value,
                    CreationDate = DateTime.Now
                };
                context.CaseInterns.Add(ci);
                context.SaveChanges();
            }
            else
            {
                //if intern is different but already exist, we just update the creation date but then be retrieved in the ddl
                if (oldVM.InternId != vm.InternId)
                {
                    CaseIntern ci = caseInternList.First();
                    ci.CreationDate = DateTime.Now;
                    context.CaseInterns.Attach(ci);
                    context.Entry(ci).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }
        private void ValidateIntakeCertifiedIntern(IntakeVM oldVM, IntakeVM vm)
        {
            List<CaseCertifiedIntern> caseInternList = context.CaseCertifiedInterns
                    .Where(c => c.InternId == vm.CertifiedInternId.Value)
                    .Where(c => c.CaseId == vm.CaseId)
                    .OrderByDescending(c => c.CreationDate)
                    .ToList();
            if (caseInternList.Count == 0)
            {
                CaseCertifiedIntern ci = new CaseCertifiedIntern
                {
                    CaseId = vm.CaseId,
                    InternId = vm.CertifiedInternId.Value,
                    CreationDate = DateTime.Now
                };
                context.CaseCertifiedInterns.Add(ci);
                context.SaveChanges();
            }
            else
            {
                //if intern is different but already exist, we just update the creation date but then be retrieved in the ddl
                if (oldVM.CertifiedInternId != vm.CertifiedInternId)
                {
                    CaseCertifiedIntern ci = caseInternList.First();
                    ci.CreationDate = DateTime.Now;
                    context.CaseCertifiedInterns.Attach(ci);
                    context.Entry(ci).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }
        private void ValidateIntakeAttorneys(IntakeVM vm)
        {
            RemoveAllAttorneysFromIntakesVM(vm);

            foreach (var attorneyId in vm.AttorneyIds)
            {
                CaseAttorney ca = new CaseAttorney
                {
                    CaseId = vm.CaseId,
                    AttorneyId = attorneyId,
                    CreationDate = DateTime.Now
                };
                context.CaseAttorneys.Add(ca);
            }
            context.SaveChanges();
        }

        private void RemoveAllAttorneysFromIntakesVM(IntakeVM vm)
        {
            var actualAttorneys = context.CaseAttorneys.Where(ca => ca.CaseId == vm.CaseId).ToList();
            context.CaseAttorneys.RemoveRange(actualAttorneys);
            context.SaveChanges();
        }

        public void IntakeDelete(IntakeVM vm)
        {
            Case obj = context.Cases.Where(c => c.CaseId == vm.CaseId).FirstOrDefault();
            context.Cases.Remove(obj);
            context.SaveChanges();
        }

        #endregion

        #region Cases
        public List<IntakeVM> GetAllCasesByStatus(string status)
        {
            return context.Cases
                .Where(c => c.CaseNo != null)
                .Where(c => c.Status.Trim().ToLower() == status.Trim().ToLower())
                .Select(c => new IntakeVM {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    CaseId = c.CaseId,
                    CaseNo = c.CaseNo,
                    PhoneList = c.Telephones.Select(t => t.Number).ToList(),
                    InternFullName = context.CaseInterns.Where(cint => cint.CaseId == c.CaseId)
                        .Select(cint => (cint.Intern.User.FirstName + " " + cint.Intern.User.LastName)).FirstOrDefault(),
                    AttorneyList = context.CaseAttorneys.Where(ca => ca.CaseId == c.CaseId).Select(a => a.Attorney.User.DisplayName).ToList(),
                    Type = (c.CaseType.TypeCode + "-" + c.CaseType.Description)
                }).ToList();
        }

        public List<IntakeVM> GetCasesByAttorneyId(int attorneyId)
        {
            var caseIds = context.CaseAttorneys
                .Where(c => c.AttorneyId == attorneyId)
                .Where(c => c.Case.CaseNo != null )
                .Where(c => c.Case.Status.ToLower().Trim() == "open")
                .Select(c => c.CaseId)
                .ToList();

            return context.Cases
                .Where(c => caseIds.Any(id => id == c.CaseId))
                .Select(c => new IntakeVM
                {
                    CaseId = c.CaseId,
                    CaseNo = c.CaseNo,
                    CertifiedInternFullName = context.CaseCertifiedInterns.Where(cint => cint.CaseId == c.CaseId)
                        .Select(cint => (cint.Intern.User.FirstName + " " + cint.Intern.User.LastName)).FirstOrDefault(),
                    InternFullName = context.CaseInterns.Where(cint => cint.CaseId == c.CaseId)
                        .Select(cint => (cint.Intern.User.FirstName + " " + cint.Intern.User.LastName)).FirstOrDefault()
                })
                .ToList();
        }
        public IntakeVM GetCaseById(int id)
        {
            IntakeVM vm = context.Cases
                .Where(c => c.CaseId == id)
                .Select(ModelHelper.GetIntakeFromModelFunc)
                .FirstOrDefault();
            if (vm != null)
            {
                vm.InternId = context.CaseInterns
                    .Where(c => c.CaseId == vm.CaseId)
                    .OrderByDescending(c => c.CreationDate)
                    .Select(c => c.InternId)
                    .FirstOrDefault();

                vm.AttorneyIds = context.Attorneys
                    .Where(a => a.CaseAttorneys.Any(ca => ca.CaseId == vm.CaseId))
                    .Select(a => a.AttorneyId)
                    .ToArray();

                vm.ReferralSources = context.ReferralSources.Where(r => r.Cases.Any(c => c.CaseId == vm.CaseId))
                        .Select(r => new ReferralSourceVM
                        {
                            ReferralSource1 = r.ReferralSource1,
                            ReferralSourceId = r.ReferralSourceId
                        }).ToList();
            }
            return vm;
        }

        public void CaseInsert(int caseId, string caseNo)
        {
            Case @case = context.Cases.Where(c => c.CaseId == caseId).FirstOrDefault();
            @case.CaseNo = caseNo;
            context.Cases.Attach(@case);
            context.Entry(@case).State = EntityState.Modified;
            context.SaveChanges();

            IntakeVM vm = GetIntakeById(caseId);
            LogIntake_MainInfo("Created", vm, vm);
        }

        public void CaseDelete(IntakeVM vm)
        {
            throw new NotImplementedException();
        }

        public void CaseUpdate(IntakeVM vm)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Telephone
        public List<TelephoneVM> GetAllTelephones()
        {
            return context.Telephones
                .Select(t => new TelephoneVM
                {
                    CaseId = t.CaseId,
                    Number = t.Number,
                    TelephoneId = t.TelephoneId,
                    Type = t.Type
                })
                .ToList();
        }

        public void TelephoneInsert(TelephoneVM vm)
        {
            Telephone t = new Telephone
            {
                CaseId = vm.CaseId,
                Number = vm.Number,
                TelephoneId = vm.TelephoneId,
                Type = vm.Type
            };
            context.Telephones.Add(t);
            context.SaveChanges();

            LogIntake_OtherInfo("Telephone Created", vm.CaseId, _user);
        }

        public void TelephoneDelete(TelephoneVM vm)
        {
            Telephone t = context.Telephones.Where(tt => tt.TelephoneId == vm.TelephoneId).FirstOrDefault();
            context.Telephones.Remove(t);
            context.SaveChanges();

            LogIntake_OtherInfo("Telephone Removed", vm.CaseId, _user);
        }

        public void TelephoneUpdate(TelephoneVM vm)
        {
            Telephone t = context.Telephones.Where(tt => tt.TelephoneId == vm.TelephoneId).FirstOrDefault();
            t.Number = vm.Number;
            t.Type = vm.Type;
            t.CaseId = vm.CaseId;
            context.Telephones.Attach(t);
            context.Entry(t).State = EntityState.Modified;
            context.SaveChanges();

            LogIntake_OtherInfo("Telephone Updated", vm.CaseId, _user);
        }

        #endregion

        #region Email
        public List<EmailVM> GetAllEmails()
        {
            return context.Emails
                .Select(e => new EmailVM
                {
                    CaseId = e.CaseId,
                    Email1 = e.Email1,
                    EmailId = e.EmailId,
                    Type = e.Type
                })
                .ToList();
        }

        public int EmailInsert(EmailVM vm)
        {
            Email e = new Email
            {
                CaseId = vm.CaseId,
                Email1 = vm.Email1,
                EmailId = vm.EmailId,
                Type = vm.Type
            };
            context.Emails.Add(e);
            context.SaveChanges();

            //just for intakes' cases' emails
            if(vm.CaseId != null)
                LogIntake_OtherInfo("Email Created", vm.CaseId, _user);

            return e.EmailId;
        }

        public void EmailDelete(EmailVM vm)
        {
            Email e = context.Emails.Where(ee => ee.EmailId == vm.EmailId).FirstOrDefault();
            context.Emails.Remove(e);
            context.SaveChanges();

            //just for intakes' cases' emails
            if (vm.CaseId != null)
                LogIntake_OtherInfo("Email Removed", vm.CaseId, _user);
        }

        public void EmailUpdate(EmailVM vm)
        {
            Email e = context.Emails.Where(ee => ee.EmailId == vm.EmailId).FirstOrDefault();
            e.CaseId = vm.CaseId;
            e.Email1 = vm.Email1;
            e.Type = vm.Type;
            context.Emails.Attach(e);
            context.Entry(e).State = EntityState.Modified;
            context.SaveChanges();

            //just for intakes' cases' emails
            if (vm.CaseId != null)
                LogIntake_OtherInfo("Email Updated", vm.CaseId, _user);
        }

        #endregion

        #region Address
        public List<AddressVM> GetAllAddresses()
        {
            return context.Addresses
                .Select(a => new AddressVM
                {
                    Address1 = a.Address1,
                    Address2 = a.Address2,
                    AddressId = a.AddressId,
                    CaseId = a.CaseId,
                    City = a.City,
                    Country = a.Country,
                    State = a.State,
                    Type = a.Type,
                    PostalCode = a.PostalCode
                })
                .ToList();
        }

        public int AddressInsert(AddressVM vm)
        {
            Address a = new Address
            {
                Address1 = vm.Address1,
                Address2 = vm.Address2,
                AddressId = vm.AddressId,
                CaseId = vm.CaseId,
                City = vm.City,
                Country = vm.Country,
                PostalCode = vm.PostalCode,
                State = vm.State,
                Type = vm.Type
            };
            context.Addresses.Add(a);
            context.SaveChanges();

            //just for intakes' cases' addresses
            if (vm.CaseId != null)
                LogIntake_OtherInfo("Address Created", vm.CaseId, _user);

            return a.AddressId;
        }

        public void AddressDelete(AddressVM vm)
        {
            Address a = context.Addresses.Where(aa => aa.AddressId == vm.AddressId).FirstOrDefault();
            context.Addresses.Remove(a);
            context.SaveChanges();

            //just for intakes' cases' addresses
            if (vm.CaseId != null)
                LogIntake_OtherInfo("Address Removed", vm.CaseId, _user);
        }

        public void AddressUpdate(AddressVM vm)
        {
            Address a = context.Addresses.Where(aa => aa.AddressId == vm.AddressId).FirstOrDefault();
            a.Address1 = vm.Address1;
            a.Address2 = vm.Address2;
            a.CaseId = vm.CaseId;
            a.City = vm.City;
            a.Country = vm.Country;
            a.PostalCode = vm.PostalCode;
            a.State = vm.State;
            a.Type = vm.Type;
            context.Addresses.Attach(a);
            context.Entry(a).State = EntityState.Modified;
            context.SaveChanges();

            //just for intakes' cases' addresses
            if (vm.CaseId != null)
                LogIntake_OtherInfo("Address Updated", vm.CaseId, _user);
        }

        #endregion

        #region CaseNotes
        public List<CaseNotesVM> GetAllCaseNotes()
        {
            return context.CaseNotes
                .Select(n => new CaseNotesVM {
                    CaseId = n.CaseId,
                    CaseNoteId = n.CaseNoteId,
                    CreatedById = n.CreatedById,
                    CreationDate = n.CreationDate,
                    Detail = n.Detail,
                    UserName = context.Users
                        .Where(u => u.UserId == n.CreatedById)
                        .Select(u => (u.FirstName ?? string.Empty) + " " +  (u.LastName ?? string.Empty))
                        .FirstOrDefault()
                })
                .ToList();
        }

        public void CaseNoteInsert(CaseNotesVM vm)
        {
            CaseNote note = new CaseNote {
                CaseId = vm.CaseId,
                CaseNoteId = vm.CaseNoteId,
                CreatedById = vm.CreatedById,
                CreationDate = DateTime.Now,
                Detail = vm.Detail?.Trim()
            };
            context.CaseNotes.Add(note);
            context.SaveChanges();

            LogIntake_OtherInfo("Case Note Created", vm.CaseId, vm.CreatedById);
        }

        public void CaseNoteDelete(CaseNotesVM vm)
        {
            CaseNote note = context.CaseNotes.Where(n => n.CaseNoteId == vm.CaseNoteId).FirstOrDefault();
            context.CaseNotes.Remove(note);
            context.SaveChanges();

            LogIntake_OtherInfo("Case Note Removed", vm.CaseId, vm.CreatedById);
        }

        public void CaseNoteUpdate(CaseNotesVM vm)
        {
            CaseNote note = context.CaseNotes.Where(n => n.CaseNoteId == vm.CaseNoteId).FirstOrDefault();
            note.CaseId = vm.CaseId;
            note.CreatedById = vm.CreatedById;
            note.CreationDate = vm.CreationDate;
            note.Detail = vm.Detail?.Trim();
            context.CaseNotes.Attach(note);
            context.Entry(note).State = EntityState.Modified;
            context.SaveChanges();

            LogIntake_OtherInfo("Case Note Updated", vm.CaseId, vm.CreatedById);
        }

        #endregion

        #region CaseParty
        public List<CasePartyVM> GetAllCaseParties()
        {
            var list = context.CaseParties.ToList();
            return list.Select(p => new CasePartyVM
                {
                    AddressId = p.AddressId,
                    CaseId = p.CaseId,
                    CasePartyId = p.CasePartyId,
                    EmailId = p.EmailId,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    OrganizationName = p.OrganizationName,
                    PartyType = p.PartyType,
                    IsIUStudent = p.IsIUStudent,
                    Address = ModelHelper.GetAddressFromModelFunc(p.Address),
                    Email = ModelHelper.GetEmailFromModelFunc(p.Email)

                })
                .ToList();
        }

        public void CasePartyInsert(CasePartyVM vm)
        {
            vm.Email.Type = "Case Party";
            vm.Address.Type = "Case Party";
            int emailId = EmailInsert(vm.Email);
            int addressId = AddressInsert(vm.Address);

            CaseParty party = new CaseParty {
                CaseId = vm.CaseId,
                CasePartyId = vm.CasePartyId,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                OrganizationName = vm.OrganizationName,
                PartyType = vm.PartyType,
                IsIUStudent = vm.IsIUStudent,
                EmailId = emailId,
                AddressId = addressId
            };
            context.CaseParties.Add(party);
            context.SaveChanges();

            LogIntake_OtherInfo("Case Party Created", vm.CaseId, _user);
        }

        public void CasePartyDelete(CasePartyVM vm)
        {
            CaseParty party = context.CaseParties.Where(p => p.CasePartyId == vm.CasePartyId).FirstOrDefault();
            context.CaseParties.Remove(party);
            context.SaveChanges();

            EmailDelete(vm.Email);
            AddressDelete(vm.Address);

            LogIntake_OtherInfo("Case Party Removed", vm.CaseId, _user);
        }

        public void CasePartyUpdate(CasePartyVM vm)
        {
            CaseParty party = context.CaseParties.Where(p => p.CasePartyId == vm.CasePartyId).FirstOrDefault();
            party.FirstName = vm.FirstName;
            party.LastName = vm.LastName;
            party.IsIUStudent = vm.IsIUStudent;
            party.PartyType = vm.PartyType;
            context.CaseParties.Attach(party);
            context.Entry(party).State = EntityState.Modified;
            context.SaveChanges();

            EmailUpdate(vm.Email);
            AddressUpdate(vm.Address);
            LogIntake_OtherInfo("Case Party Updated", vm.CaseId, _user);

        }
        #endregion

        #region CaseDocument
        public List<CaseDocumentVM> GetAllCaseDocuments()
        {
            return context.CaseDocuments
                .Select(d => new CaseDocumentVM {
                    CaseDocumentId = d.CaseDocumentId,
                    CaseId = d.CaseId,
                    CreatedById = d.CreatedById,
                    CreationDate = d.CreationDate,
                    Filename = d.Filename,
                    IsWorldox = d.IsWorldox,
                    CreatedBy = d.User.UserName,
                    FileType = d.FileType
                })
                .ToList();
        }

        public void CaseDocumentInsert(CaseDocumentVM vm)
        {
            CaseDocument doc = new CaseDocument {
                CaseId = vm.CaseId,
                CreatedById = vm.CreatedById,
                CreationDate = DateTime.Now,
                Filename = vm.Filename,
                IsWorldox = vm.IsWorldox,
                FileType = vm.FileType
            };
            context.CaseDocuments.Add(doc);
            context.SaveChanges();

            LogIntake_OtherInfo("Case Document Created", vm.CaseId, vm.CreatedById);
        }

        public void CaseDocumentDelete(CaseDocumentVM vm)
        {
            CaseDocument doc = context.CaseDocuments.Where(d => d.CaseDocumentId == vm.CaseDocumentId).FirstOrDefault();
            context.CaseDocuments.Remove(doc);
            context.SaveChanges();

            LogIntake_OtherInfo("Case Document Removed", vm.CaseId, vm.CreatedById);
        }

        public void CaseDocumentUpdate(CaseDocumentVM vm)
        {
            CaseDocument doc = context.CaseDocuments.Where(d => d.CaseDocumentId == vm.CaseDocumentId).FirstOrDefault();
            doc.CaseId = vm.CaseId;
            doc.CreatedById = vm.CreatedById;
            doc.CreationDate = vm.CreationDate;
            doc.Filename = vm.Filename;
            doc.IsWorldox = vm.IsWorldox;
            doc.FileType = vm.FileType;
            context.CaseDocuments.Attach(doc);
            context.Entry(doc).State = EntityState.Modified;
            context.SaveChanges();

            LogIntake_OtherInfo("Case Document Updated", vm.CaseId, vm.CreatedById);
        }
        #endregion
        
        #region CaseMoney
        public List<CaseMoneyVM> GetAllCaseMoneys()
        {
            return context.CaseMoneys
                .Select(m => new CaseMoneyVM
                {
                    Amount = m.Amount,
                    CaseId = m.CaseId,
                    CaseMoneyId = m.CaseMoneyId,
                    CreatedById = m.CreatedById,
                    CreationDate = m.CreationDate,
                    Type = m.Type,
                    CreatedBy = context.Users
                        .Where(u => u.UserId == m.CreatedById)
                        .Select(u => (u.FirstName ?? string.Empty) + " " + (u.LastName ?? string.Empty))
                        .FirstOrDefault()
                })
                .ToList();
        }

        public void CaseMoneyInsert(CaseMoneyVM vm)
        {
            CaseMoney money = new CaseMoney {
                Amount = vm.Amount,
                CaseId = vm.CaseId,
                CreatedById = vm.CreatedById,
                CreationDate = DateTime.Now,
                Type = vm.Type
            };
            context.CaseMoneys.Add(money);
            context.SaveChanges();

            string text = "Case Money '" + (vm.Type ?? string.Empty) + "' Created - " + (vm.Amount.ToString("C"));
            LogIntake_OtherInfo(text , vm.CaseId, vm.CreatedById);
        }

        public void CaseMoneyDelete(CaseMoneyVM vm)
        {
            CaseMoney money = context.CaseMoneys.Where(m => m.CaseMoneyId == vm.CaseMoneyId).FirstOrDefault();
            context.CaseMoneys.Remove(money);
            context.SaveChanges();

            string text = "Case Money '" + (vm.Type ?? string.Empty) + "' Removed - " + (vm.Amount.ToString("C"));
            LogIntake_OtherInfo(text, vm.CaseId, vm.CreatedById);
        }

        public void CaseMoneyUpdate(CaseMoneyVM vm)
        {
            CaseMoney money = context.CaseMoneys.Where(m => m.CaseMoneyId == vm.CaseMoneyId).FirstOrDefault();
            money.Amount = vm.Amount;
            money.CaseId = vm.CaseId;
            money.CreatedById = vm.CreatedById;
            money.CreationDate = vm.CreationDate;
            money.Type = vm.Type;
            context.CaseMoneys.Attach(money);
            context.Entry(money).State = EntityState.Modified;
            context.SaveChanges();

            string text = "Case Money '" + (vm.Type ?? string.Empty) + "' Updated - " + (vm.Amount.ToString("C"));
            LogIntake_OtherInfo(text, vm.CaseId, vm.CreatedById);
        }
        #endregion

        #region ReferralSources
        public List<ReferralSourceVM> GetAllReferralSources()
        {
            return context.ReferralSources
                .Select(r => new ReferralSourceVM
                {
                    ReferralSource1 = r.ReferralSource1,
                    ReferralSourceId = r.ReferralSourceId
                })
                .ToList();
        }

        public ReferralSourceVM GetReferralSourcesById(int id)
        {
            return context.ReferralSources
                .Select(r => new ReferralSourceVM
                {
                    ReferralSource1 = r.ReferralSource1,
                    ReferralSourceId = r.ReferralSourceId
                }).FirstOrDefault();
        }

        public void ReferralSourcesUpdate(int caseId, int referralId)
        {
            Case @case = context.Cases.Where(c => c.CaseId == caseId).FirstOrDefault();
            ReferralSource referral = context.ReferralSources.Where(r => r.ReferralSourceId == referralId).FirstOrDefault();
            bool contain = @case.ReferralSources.Any(r => r.ReferralSourceId == referralId);
            if (contain)
                @case.ReferralSources.Remove(referral);
            else
                @case.ReferralSources.Add(referral);

            context.Cases.Attach(@case);
            context.Entry(@case).State = EntityState.Modified;
            context.SaveChanges();

            if(contain)
                LogIntake_OtherInfo("Referral Source '" + (referral.ReferralSource1 ?? string.Empty)+ "' Un-checked", caseId, _user);
            else
                LogIntake_OtherInfo("Referral Source '" + (referral.ReferralSource1 ?? string.Empty) + "' Checked", caseId, _user);
        }
        #endregion

        #region GenericValues
        public List<GenericValuesLookupVM> GetAllGenericValuesByType(string type)
        {
            return context.GenericValuesLookups
                .Where(g => g.Type == type)
                .Select(g => new GenericValuesLookupVM
                {
                    Id = g.Id,
                    Display = g.Display,
                    Type = g.Type,
                    Value = g.Value
                })
                .ToList();
        }

        #endregion
        public void Dispose()
        {
            context.Dispose();
        }

    }
}