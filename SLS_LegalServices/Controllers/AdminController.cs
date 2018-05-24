using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SLS_LegalServices.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Attorneys()
        {
            return PartialView("_Attorneys");
        }

        public ActionResult Interns()
        {
            return View();
        }

        public ActionResult CaseTypes()
        {
            return PartialView("_CaseTypes");
        }

        public ActionResult SystemUsers()
        {
            return PartialView("_SystemUsers");
        }

        public ActionResult CaseLog()
        {
            return View();
        }
    }
}