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
    public class CasesController : Controller
    {
        private IMainRepository repository;

        public CasesController(IMainRepository mainRepository)
        {
            repository = mainRepository;
        }

        public ActionResult Index()
        {
            return View("CasesTabs");
        }

        public ActionResult Advised()
        {
            ViewBag.Status = "Advised";
            return PartialView("_Index");
        }

        public ActionResult Closed()
        {
            ViewBag.Status = "Closed";
            return PartialView("_Index");
        }

        public ActionResult NoAction()
        {
            ViewBag.Status = "NoAction";
            return PartialView("_Index");
        }

        public ActionResult Open()
        {
            ViewBag.Status = "Open";
            return PartialView("_Index");
        }

        public ActionResult Pending()
        {
            ViewBag.Status = "Pending";
            return PartialView("_Index");
        }

        public JsonResult Read([DataSourceRequest]DataSourceRequest request,string status)
        {
            var cases = repository.GetAllCasesByStatus(status);
            DataSourceResult result = cases.AsQueryable().ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details(int id)
        {
            var views = new List<KeyValuePair<int, string>>() {
                new KeyValuePair<int,string>(1,"_CasesGeneralView"),
                new KeyValuePair<int,string>(2,"_NotesView"),
                new KeyValuePair<int,string>(3,"_ContactsView"),
                new KeyValuePair<int,string>(4,"_PartiesView"),
                new KeyValuePair<int,string>(5,"_ReferralSourcesView"),
                new KeyValuePair<int,string>(6,"_NarrativeView"),
                new KeyValuePair<int,string>(7,"_DocumentsView"),
                new KeyValuePair<int,string>(8,"_MoneyView"),
                new KeyValuePair<int,string>(9,"_LogView")
            };
            var intake = repository.GetIntakeById(id);

            ViewBag.SectionList = views.OrderBy(v => v.Key).Select(v => v.Value).ToList();
            ViewBag.Attorneys = repository.GetAllAttorneys();
            ViewBag.ReferralSources = repository.GetAllReferralSources();
            ViewBag.PartyTypes = repository.GetAllGenericValuesByType("PartyType");
            ViewBag.CaseStatuses = repository.GetAllGenericValuesByType("CaseStatus");
            ViewBag.Genders = repository.GetAllGenericValuesByType("Gender");
            ViewBag.AcademicStatuses = repository.GetAllGenericValuesByType("AcademicStatus");
            ViewBag.SocialStatuses = repository.GetAllGenericValuesByType("SocialStatus");
            ViewBag.CitizenshipStatuses = repository.GetAllGenericValuesByType("CitizenshipStatus");


            if (intake != null)
                repository.LogIntake_MainInfo("Viewed", intake, intake);

            return View(intake);
        }

        public JsonResult GetInterns(string text)
        {
            var interns = repository.GetAllInterns();

            if (!string.IsNullOrEmpty(text))
            {
                interns = interns.Where(s => s.FullName.ToUpper().Contains(text.ToUpper())).ToList();
            }

            return Json(interns, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTypes(string text)
        {
            var types = repository.GetAllCaseTypes();

            if (!string.IsNullOrEmpty(text))
            {
                types = types.Where(s => s.FullDescription.ToUpper().Contains(text.ToUpper())).ToList();
            }

            return Json(types, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetIntakeById(int id)
        {
            var intake = repository.GetIntakeById(id);
            return Json(intake, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(int caseId, string caseNo)
        {
            repository.CaseInsert(caseId, caseNo);
            return Json(caseId);
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

        public JsonResult GetCasesByAttorneyId([DataSourceRequest]DataSourceRequest request, int attorneyId)
        {
            var cases = repository.GetCasesByAttorneyId(attorneyId);
            DataSourceResult result = cases.AsQueryable().ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            repository.Dispose();
            base.Dispose(disposing);
        }
    }
}