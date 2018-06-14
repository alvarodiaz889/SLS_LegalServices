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
        private IUserRepository UserRepository = new UserRepositoryImpl();
        
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
                IntakeUpdate(vm);
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
            obj.Narrative = vm.Narrative;

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
        }

        public void TelephoneDelete(TelephoneVM vm)
        {
            Telephone t = context.Telephones.Where(tt => tt.TelephoneId == vm.TelephoneId).FirstOrDefault();
            context.Telephones.Remove(t);
            context.SaveChanges();
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
            return e.EmailId;
        }

        public void EmailDelete(EmailVM vm)
        {
            Email e = context.Emails.Where(ee => ee.EmailId == vm.EmailId).FirstOrDefault();
            context.Emails.Remove(e);
            context.SaveChanges();
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
            return a.AddressId;
        }

        public void AddressDelete(AddressVM vm)
        {
            Address a = context.Addresses.Where(aa => aa.AddressId == vm.AddressId).FirstOrDefault();
            context.Addresses.Remove(a);
            context.SaveChanges();
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
        }

        public void CaseNoteDelete(CaseNotesVM vm)
        {
            CaseNote note = context.CaseNotes.Where(n => n.CaseNoteId == vm.CaseNoteId).FirstOrDefault();
            context.CaseNotes.Remove(note);
            context.SaveChanges();
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
        }

        public void CasePartyDelete(CasePartyVM vm)
        {
            CaseParty party = context.CaseParties.Where(p => p.CasePartyId == vm.CasePartyId).FirstOrDefault();
            context.CaseParties.Remove(party);
            context.SaveChanges();

            EmailDelete(vm.Email);
            AddressDelete(vm.Address);
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

        }

        #endregion
    }
}