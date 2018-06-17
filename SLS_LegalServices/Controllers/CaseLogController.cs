using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SLS_LegalServices.Repositories;
using SLS_LegalServices.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SLS_LegalServices.Controllers
{
    public class CaseLogController : Controller
    {
        IMainRepository repository;

        public CaseLogController(IMainRepository repository)
        {
            this.repository = repository;
        }
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            List<LogVM> casetypes = repository.GetAllCaseLogs();
            DataSourceResult result = casetypes.AsQueryable().ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadById([DataSourceRequest]DataSourceRequest request, int caseId)
        {
            List<LogVM> casetypes = repository.GetAllCaseLogsByCaseId(caseId);
            DataSourceResult result = casetypes.AsQueryable().ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, LogVM obj)
        {
            if (ModelState.IsValid)
            {
                repository.CaseLogInsert(obj);
            }

            return Json(new[] { obj }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, LogVM obj)
        {
            repository.CaseLogDelete(obj);
            return Json(new[] { obj }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, LogVM obj)
        {
            if (ModelState.IsValid)
            {
                repository.CaseLogUpdate(obj);
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