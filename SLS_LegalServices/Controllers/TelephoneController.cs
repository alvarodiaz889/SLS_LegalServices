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
    public class TelephoneController : Controller
    {
        IMainRepository repository;

        public TelephoneController(IMainRepository repository)
        {
            this.repository = repository;
        }
        public ActionResult Read([DataSourceRequest]DataSourceRequest request, int intakeId)
        {
            var telephones = repository.GetAllTelephones().Where(t => t.CaseId == intakeId).ToList();
            DataSourceResult result = telephones.AsQueryable().ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, TelephoneVM obj)
        {
            if (ModelState.IsValid)
            {
                repository.SetLoggedUserId(Guid.Parse(User.Identity.GetUserId()));
                repository.TelephoneInsert(obj);
            }

            return Json(new[] { obj }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, TelephoneVM obj)
        {
            repository.SetLoggedUserId(Guid.Parse(User.Identity.GetUserId()));
            repository.TelephoneDelete(obj);
            return Json(new[] { obj }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, TelephoneVM obj)
        {
            if (ModelState.IsValid)
            {
                repository.SetLoggedUserId(Guid.Parse(User.Identity.GetUserId()));
                repository.TelephoneUpdate(obj);
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