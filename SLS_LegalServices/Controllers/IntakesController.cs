using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SLS_LegalServices.Repositories;
using SLS_LegalServices.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SLS_LegalServices.Controllers
{
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
            var types = context.CaseTypes.ToList();

            if (!string.IsNullOrEmpty(text))
            {
                types = types.Where(s => s.Description.ToUpper().Contains(text.ToUpper())).ToList();
            }

            return Json(types, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            repository.Dispose();
            base.Dispose(disposing);
        }
    }
}