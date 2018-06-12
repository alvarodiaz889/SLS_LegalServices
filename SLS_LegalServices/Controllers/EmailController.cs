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
    public class EmailController : Controller
    {
        IMainRepository repository;

        public EmailController(IMainRepository repository)
        {
            this.repository = repository;
        }
        public ActionResult Read([DataSourceRequest]DataSourceRequest request, int intakeId)
        {
            var emails = repository.GetAllEmails().Where(e => e.CaseId == intakeId).ToList();
            DataSourceResult result = emails.AsQueryable().ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, EmailVM obj)
        {
            if (ModelState.IsValid)
            {
                repository.EmailInsert(obj);
            }

            return Json(new[] { obj }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, EmailVM obj)
        {
            repository.EmailDelete(obj);
            return Json(new[] { obj }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, EmailVM obj)
        {
            if (ModelState.IsValid)
            {
                repository.EmailUpdate(obj);
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