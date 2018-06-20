using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using SLS_LegalServices.Repositories;
using SLS_LegalServices.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SLS_LegalServices.Controllers
{
    [Authorize]
    public class IntakesController : Controller
    {
        private IMainRepository repository;
        SLS_LegalServicesEntities context = new SLS_LegalServicesEntities();

        public IntakesController(IMainRepository mainRepository)
        {
            repository = mainRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var intakes = repository.GetAllIntakes();
            DataSourceResult result = intakes.AsQueryable().ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet); 
        }
        public ActionResult Details(int id)
        {
            var views = new List<KeyValuePair<int, string>>() {
                new KeyValuePair<int,string>(1,"_GeneralView"),
                new KeyValuePair<int,string>(2,"_NotesView"),
                new KeyValuePair<int,string>(3,"_ContactsView"),
                new KeyValuePair<int,string>(4,"_PartiesView"),
                new KeyValuePair<int,string>(5,"_ReferralSourcesView"),
                new KeyValuePair<int,string>(6,"_NarrativeView"),
                new KeyValuePair<int,string>(7,"_DocumentsView"),
                new KeyValuePair<int,string>(8,"_MoneyView"),
                new KeyValuePair<int,string>(9,"_LogView")
            };
            ViewBag.SectionList = views.OrderBy(v => v.Key).Select(v => v.Value).ToList();
            var intake = repository.GetIntakeById(id);

            // data for the DDLookup editor template for attorneys and casetype in _GeneralView view
            ViewData["TypeId"] = intake?.TypeId ?? 0;
            ViewData["InternId"] = intake?.InternId ?? 0;
            ViewBag.Attorneys = repository.GetAllAttorneys();

            // data for the _ReferralSourcesView view
            ViewBag.ReferralSources = repository.GetAllReferralSources();

            //data for the DDForGrid editor template - more can be added dynamically in order to reuse the control
            //first parameter is the property name and the other is the collection
            var PropertyName_List = new Dictionary<string, object>();
            PropertyName_List.Add("PartyType", repository.GetAllGenericValuesByType("PartyType"));
            ViewBag.PropertyName_List = PropertyName_List;


            // log the info as viewed
            if(intake != null)
                repository.LogIntake_MainInfo("Viewed", intake, null);

            return View(intake);
        }

        public JsonResult GetInterns(string text)
        {
            var interns = repository.GetAllInterns();

            if (!string.IsNullOrEmpty(text))
            {
                interns = interns.Where(s => s.LastName.Contains(text)).ToList();
            }

            return Json(interns, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTypes(string text)
        {
            var types = repository.GetAllCaseTypes();

            if (!string.IsNullOrEmpty(text))
            {
                types = types.Where(s => s.Description.ToUpper().Contains(text.ToUpper())).ToList();
            }

            return Json(types, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetIntakeById(int id)
        {
            var intake = repository.GetIntakeById(id);
            return Json(intake, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(IntakeVM obj)
        {
            int id = 0;

            if (ModelState.IsValid)
            {
                obj.CreatedById = Guid.Parse(User.Identity.GetUserId());
                id = repository.IntakeInsert(obj);
            }
            
            return Json(id);
        }

        [HttpPost]
        public ActionResult Update(IntakeVM obj)
        {
            if (ModelState.IsValid)
            {
                repository.IntakeUpdate(obj);
            }
            
            return Json(obj.CaseId);
        }

        [HttpPost]
        public ActionResult Destroy(IntakeVM obj)
        {
            if (ModelState.IsValid)
            {
                repository.IntakeDelete(obj);
            }

            return Json(obj.CaseId);
        }

        [HttpPost]
        public ActionResult EditReferralSources(int caseId, int referralId)
        {
            if (ModelState.IsValid)
            {
                repository.SetLoggedUserId(Guid.Parse(User.Identity.GetUserId()));
                repository.ReferralSourcesUpdate(caseId, referralId);
            }

            var referrals = context.ReferralSources.Where(r => r.Cases.Any(c => c.CaseId == caseId))
                .Select(r => r.ReferralSourceId).ToArray();
            return Json(referrals);
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            repository.Dispose();
            base.Dispose(disposing);
        }
    }
}