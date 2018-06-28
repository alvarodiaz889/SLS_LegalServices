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
            return View();
        }
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var schedules = new List<ScheduleVM>()
            {
                new ScheduleVM{ Title = "Title 1", Description = "Description 1", ScheduleId = 1, End = DateTime.Now.AddHours(1), Start = DateTime.Now },
                new ScheduleVM{ Title = "Title 2", Description = "Description 2", ScheduleId = 2, End = DateTime.Now.AddHours(2), Start = DateTime.Now.AddHours(1) }
            };
            DataSourceResult result = schedules.AsQueryable().ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInterns([DataSourceRequest]DataSourceRequest request)
        {
            var interns = repository.GetAllInterns();
            DataSourceResult result = interns.AsQueryable().ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, ScheduleVM obj)
        {
            if (ModelState.IsValid)
            {
            }

            return Json(new[] { obj }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, ScheduleVM obj)
        {
            return Json(new[] { obj }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, ScheduleVM obj)
        {
            if (ModelState.IsValid)
            {
                
            }
            return Json(new[] { obj }.ToDataSourceResult(request, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            repository.Dispose();
            base.Dispose(disposing);
        }
    }
}