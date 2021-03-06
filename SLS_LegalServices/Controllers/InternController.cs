﻿using Kendo.Mvc.Extensions;
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
    public class InternController : Controller
    {
        private IMainRepository repository;

        public InternController(IMainRepository repository)
        {
            this.repository = repository;
        }
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            List<InternVM> casetypes = repository.GetAllInterns();
            DataSourceResult result = casetypes.AsQueryable().ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, InternVM obj)
        {
            if (ModelState.IsValid)
            {
                repository.InternInsert(obj);
            }

            return Json(new[] { obj }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, InternVM obj)
        {
            repository.InternDelete(obj);
            return Json(new[] { obj }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, InternVM obj)
        {
            if (ModelState.IsValid)
            {
                repository.InternUpdate(obj);
            }
            else {
                // app is generating an error when binding the StartTime and EndTime
                // but like those properties are managed in the nested grid they're being ignored here
                int count = 0;
                foreach (var key in ModelState.Keys)
                {
                    if (!key.Contains("StartTime") && !key.Contains("EndTime"))
                    {
                        if (ModelState[key].Errors.Count > 0)
                            count++;
                    }
                }
                if (count == 0)
                {
                    obj.Schedules = repository.GetAllScheduleByIntern(obj);
                    repository.InternUpdate(obj);
                }
            }
            return Json(new[] { obj }.ToDataSourceResult(request, ModelState));
        }
    }
}