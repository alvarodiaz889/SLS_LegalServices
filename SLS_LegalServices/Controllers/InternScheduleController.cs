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
    public class InternScheduleController : Controller
    {
        private IMainRepository mainRepository;

        public InternScheduleController(IMainRepository mainRepository)
        {
            this.mainRepository = mainRepository;
        }

        public JsonResult Read([DataSourceRequest]DataSourceRequest request)
        {
            List<InternScheduleVM> users = mainRepository.GetAllInternSchedules();
            DataSourceResult result = users.AsQueryable().ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, InternScheduleVM vm)
        {
            if (ModelState.IsValid)
            {
                mainRepository.InternScheduleInsert(vm);
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, InternScheduleVM vm)
        {
            mainRepository.InternScheduleDelete(vm);
            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, InternScheduleVM vm)
        {
            if (ModelState.IsValid)
            {
                mainRepository.InternScheduleUpdate(vm);
            }
            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        

        protected override void Dispose(bool disposing)
        {
            mainRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}