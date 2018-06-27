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
    public class CasePartyController : Controller
    {
        IMainRepository repository;

        public CasePartyController(IMainRepository repository)
        {
            this.repository = repository;
        }
        public ActionResult Read([DataSourceRequest]DataSourceRequest request, int intakeId)
        {
            var parties = repository.GetAllCaseParties().Where(e => e.CaseId == intakeId).ToList();
            DataSourceResult result = parties.AsQueryable().ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, CasePartyVM obj)
        {

            if (ModelState.IsValid)
            {
                repository.SetLoggedUserId(Guid.Parse(User.Identity.GetUserId()));
                repository.CasePartyInsert(obj);
            }

            return Json(new[] { obj }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, CasePartyVM obj)
        {
            repository.SetLoggedUserId(Guid.Parse(User.Identity.GetUserId()));
            repository.CasePartyDelete(obj);
            return Json(new[] { obj }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, CasePartyVM obj)
        {
            if (ModelState.IsValid)
            {
                repository.SetLoggedUserId(Guid.Parse(User.Identity.GetUserId()));
                repository.CasePartyUpdate(obj);
            }
            return Json(new[] { obj }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult GetPartyTypes(string text)
        {
            var partyTypes = repository.GetAllGenericValuesByType("PartyType");

            if (!string.IsNullOrEmpty(text))
            {
                partyTypes = partyTypes.Where(pt => pt.Display.Contains(text)).ToList();
            }

            return Json(partyTypes, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            repository.Dispose();
            base.Dispose(disposing);
        }
    }
}