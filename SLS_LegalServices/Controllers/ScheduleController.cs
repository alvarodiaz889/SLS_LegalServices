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
    public class ScheduleController : Controller
    {
        private readonly IMainRepository repository;

        public ScheduleController(IMainRepository repository)
        {
            this.repository = repository;
        }

        public ActionResult Index()
        {
            ViewBag.Interns = repository.GetAllInterns();
            return View();
        }
        public ActionResult Read([DataSourceRequest]DataSourceRequest request, string date)
        {
            var schedules = repository.GetAllCaseApptByDay(date);
            DataSourceResult result = schedules.AsQueryable().ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetInternSchedule(string date)
        {
            var internSchedules = repository.GetInternSchedulesByInternAndDay(date);
            return Json(internSchedules);
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, CaseApptVM obj)
        {
            if (ModelState.IsValid)
            {
                repository.CaseApptInsert(obj);
            }

            return Json(new[] { obj }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, CaseApptVM obj)
        {
            if (ModelState.IsValid)
            {
                repository.CaseApptUpdate(obj);
            }
            return Json(new[] { obj }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, CaseApptVM obj)
        {
            repository.CaseApptDelete(obj);
            return Json(new[] { obj }.ToDataSourceResult(request, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            repository.Dispose();
            base.Dispose(disposing);
        }
    }
}