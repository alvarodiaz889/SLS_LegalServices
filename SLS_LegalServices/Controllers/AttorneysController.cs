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
    public class AttorneysController : Controller
    {
        private IMainRepository repository;

        public AttorneysController(IMainRepository repository)
        {
            this.repository = repository;
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            List<AttorneyVM> casetypes = repository.GetAllAttorneys();
            DataSourceResult result = casetypes.AsQueryable().ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, AttorneyVM obj)
        {
            if (ModelState.IsValid)
            {
                repository.AttorneyInsert(obj);
            }

            return Json(new[] { obj }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, AttorneyVM obj)
        {
            repository.AttorneyDelete(obj);
            return Json(new[] { obj }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, AttorneyVM obj)
        {
            if (ModelState.IsValid)
            {
                repository.AttorneyUpdate(obj);
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
