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
    public class CaseMoneyController : Controller
    {
        IMainRepository repository;

        public CaseMoneyController(IMainRepository repository)
        {
            this.repository = repository;
        }
        public ActionResult Read([DataSourceRequest]DataSourceRequest request, int intakeId, string type)
        {
            var money = repository.GetAllCaseMoneys()
                .Where(e => e.CaseId == intakeId)
                .ToList();
            money = money.Where(e => e.Type == type).ToList();
            DataSourceResult result = money.AsQueryable().ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadTotal(int intakeId)
        {
            var money = repository.GetAllCaseMoneys()
                .Where(e => e.CaseId == intakeId)
                .ToList();
            var totalSaved = money.Where(m => m.Type == "Saved").Select(m => m.Amount).Sum().ToString("C");
            var totalCollectedRecovered = money.Where(m => m.Type == "Collected/Recovered").Select(m => m.Amount).Sum().ToString("C"); 
            var totalAttorneyFees = money.Where(m => m.Type == "Attorney Fees").Select(m => m.Amount).Sum().ToString("C");

            var result = new { totalSaved, totalCollectedRecovered, totalAttorneyFees };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, CaseMoneyVM obj)
        {
            if (ModelState.IsValid)
            {
                repository.CaseMoneyInsert(obj);
            }

            return Json(new[] { obj }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, CaseMoneyVM obj)
        {
            repository.CaseMoneyDelete(obj);
            return Json(new[] { obj }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, CaseMoneyVM obj)
        {
            if (ModelState.IsValid)
            {
                repository.CaseMoneyUpdate(obj);
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