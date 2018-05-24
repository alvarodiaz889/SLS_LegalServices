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

namespace SLS_LegalServices.Controllers
{
    public class AttorneysController : Controller
    {
        private SLS_LegalServicesEntities db = new SLS_LegalServicesEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<Attorney> attorneys = db.Attorneys;
            DataSourceResult result = attorneys.ToDataSourceResult(request, attorney => new {
                AttorneyId = attorney.AttorneyId,
            });

            return Json(result);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
