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
        private readonly IUserRepository UserRepository = new UserRepositoryImpl();
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
                UserName = o.User.UserName,
                CaseCode = o.Case.CaseNo
            }).ToList();
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
            switch (action)
            {
                case "Viewed": 
                    log = new LogVM
                    {
                        Action = "Viewed",
                        Active = 1,
                        CaseId = old.CaseId,
                        CreatedById = old.CreatedById,
                        LogDate = DateTime.Now,
                        LogType = "intakes",
                        Detail = ""
                    };
                    CaseLogInsert(log);
                    break;

                case "Created":
                    log = new LogVM
                    {
                        Action = "Created",
                        Active = 1,
                        CaseId = recent.CaseId,
                        CreatedById = recent.CreatedById,
                        LogDate = DateTime.Now,
                        LogType = "intakes",
                        Detail = ""
                    };
                    CaseLogInsert(log);
                    break;

                case "Updated":

                    StringBuilder detail = new StringBuilder("");

                    if (old.FirstName != recent.FirstName) detail.AppendLine("First Name Changed.");
                    if (old.LastName != recent.LastName) detail.AppendLine("Last Name Changed.");
                    if (old.IUStudentId != recent.IUStudentId) detail.AppendLine("IU Student Id Changed.");
                    if (old.TypeId != recent.TypeId) detail.AppendLine("Type Changed.");
                    if (old.Narrative != recent.Narrative) detail.AppendLine("Narrative Changed.");

                    if (old.InternId != recent.InternId)
                    {
                        //in case the record one record has 0 and the other null. Doesn't have to log it
                        var InternIdOne = old.InternId.HasValue && old.InternId != 0;
                        var InternIdTwo = recent.InternId.HasValue && recent.InternId != 0;
                        if (InternIdOne || InternIdTwo) detail.AppendLine("Intern Changed.");
                    }

                    var AttorneysOne = old.Attorneys != null && old.Attorneys.Count > 0;
                    var AttorneysTwo = recent.AttorneyIds != null && recent.AttorneyIds.Length > 0;
                    if (AttorneysOne && AttorneysTwo)
                    {
                        HashSet<int> ids = new HashSet<int>();
                        foreach (var att in old.Attorneys)
                            ids.Add(att.AttorneyId);
                        foreach (var att in recent.AttorneyIds)
                            ids.Add(att);
                        if (ids.Count() != old.Attorneys.Count)
                            detail.AppendLine("Attorney Changed.");
                        
                    }
                    else if (AttorneysOne || AttorneysTwo)
                    {
                        detail.AppendLine("Attorney Changed.");
                    }

                    log = new LogVM
                    {
                        Action = "Updated",
                        Active = 1,
                        CaseId = recent.CaseId,
                        CreatedById = old.CreatedById,
                        LogDate = DateTime.Now,
                        LogType = "intakes",
                        Detail = detail.ToString()
                    };
                    CaseLogInsert(log);
                    break;
            }
        }

        private void LogIntake_OtherInfo(string detail, int? caseId, Guid createdById)
        {
            LogVM log = new LogVM
            {
                Action = "Updated",
                Active = 1,
                CaseId = caseId,
                CreatedById = createdById,
                LogDate = DateTime.Now,
                LogType = "intakes",
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
                .Select(ModelHelper.GetIntakeFromModelFunc)
                .ToList();
        }

        public IntakeVM GetIntakeById(int id)
        {
            IntakeVM vm =  context.Cases
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

                vm.Attorneys = context.Attorneys
                    .Where(a => a.CaseAttorneys.Any(ca => ca.CaseId == vm.CaseId))
                    .Select(ModelHelper.GetAttorneyFromModel)
                    .ToList();

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
            caseObj.Status = "Open";
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

        private void IntakeInsertUpdate(bool insert, IntakeVM vm)
        {
            if(insert)
                LogIntake_MainInfo("Created", null, vm);
            else
                LogIntake_MainInfo("Updated", GetIntakeById(vm.CaseId), vm);

            Case obj = context.Cases.Where(c => c.CaseId == vm.CaseId).FirstOrDefault();
            obj.FirstName = vm.FirstName;
            obj.LastName = vm.LastName;
            obj.IUStudentId = vm.IUStudentId;
            obj.TypeId = vm.TypeId;
            obj.Narrative = vm.Narrative;
            //obj.Status = vm.Status;

            context.Cases.Attach(obj);
            context.Entry(obj).State = EntityState.Modified;
            context.SaveChanges();

            if (vm.InternId.HasValue)
            {
                List<CaseIntern> caseInternList = context.CaseInterns
                    .Where(c => c.InternId == vm.InternId.Value)
                    .Where(c => c.CaseId == vm.CaseId)
                    .ToList();
                if (caseInternList.Count == 0)
                {
                    CaseIntern ci = new CaseIntern
                    {
                        CaseId = obj.CaseId,
                        InternId = vm.InternId.Value,
                        CreationDate = DateTime.Now
                    };
                    context.CaseInterns.Add(ci);
                    context.SaveChanges();
                }
            }

            if (vm.AttorneyIds != null)
            {
                var actualAttorneys = context.CaseAttorneys.Where(ca => ca.CaseId == vm.CaseId).ToList();
                context.CaseAttorneys.RemoveRange(actualAttorneys);
                context.SaveChanges();

                foreach (var attorneyId in vm.AttorneyIds)
                {
                    CaseAttorney ca = new CaseAttorney
                    {
                        CaseId = obj.CaseId,
                        AttorneyId = attorneyId,
                        CreationDate = DateTime.Now
                    };
                    context.CaseAttorneys.Add(ca);
                }
                context.SaveChanges();
            }
        }




        public void IntakeUpdate(IntakeVM vm)
        {
            IntakeInsertUpdate(false, vm);
        }

        public void IntakeDelete(IntakeVM vm)
        {
            Case obj = context.Cases.Where(c => c.CaseId == vm.CaseId).FirstOrDefault();
            context.Cases.Remove(obj);
            context.SaveChanges();
        }

        #endregion

        #region Cases
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

            LogIntake_OtherInfo("Case Money '" + (vm.Type ?? string.Empty) + "' Created", vm.CaseId, vm.CreatedById);
        }

        public void CaseMoneyDelete(CaseMoneyVM vm)
        {
            CaseMoney money = context.CaseMoneys.Where(m => m.CaseMoneyId == vm.CaseMoneyId).FirstOrDefault();
            context.CaseMoneys.Remove(money);
            context.SaveChanges();

            LogIntake_OtherInfo("Case Money '" + (vm.Type ?? string.Empty) + "' Removed", vm.CaseId, vm.CreatedById);
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

            LogIntake_OtherInfo("Case Money '" + (vm.Type ?? string.Empty) + "' Updated", vm.CaseId, vm.CreatedById);
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

        public void Dispose()
        {
            context.Dispose();
        }

        
    }
}