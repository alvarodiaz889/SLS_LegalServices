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
    }
}