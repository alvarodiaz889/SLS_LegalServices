﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SLS_LegalServices;
using SLS_LegalServices.Repositories;
using SLS_LegalServices.ViewModels;

namespace SLS_LegalServices.Controllers
{
    public class CaseTypesController : Controller
    {
        private SLS_LegalServicesEntities db = new SLS_LegalServicesEntities();
        private IMainRepository repository = new MainRepository();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            List<CaseTypesVM> casetypes = repository.GetAllCaseTypes();
            DataSourceResult result = casetypes.AsQueryable().ToDataSourceResult(request);

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, CaseTypesVM obj)
        {
            if (ModelState.IsValid)
            {
                repository.CaseTypesInsert(obj);
            }

            return Json(new[] { obj }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, CaseTypesVM obj)
        {
            repository.CaseTypesDelete(obj);
            return Json(new[] { obj }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, CaseTypesVM obj)
        {
            if (ModelState.IsValid)
            {
                repository.CaseTypesUpdate(obj);
            }
            return Json(new[] { obj }.ToDataSourceResult(request, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
