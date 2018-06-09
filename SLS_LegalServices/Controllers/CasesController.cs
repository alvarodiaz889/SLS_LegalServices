using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SLS_LegalServices.Controllers
{
    public class CasesController : Controller
    {
        // GET: Case
        public ActionResult Index()
        {
             var views = new List<KeyValuePair<int,string>>() {
                new KeyValuePair<int,string>(1,"_GeneralView"),
                new KeyValuePair<int,string>(2,"_NotessView"),
                new KeyValuePair<int,string>(3,"_ContactsView"),
                new KeyValuePair<int,string>(4,"_PartiesView"),
                new KeyValuePair<int,string>(5,"_ReferralSourcesView"),
                new KeyValuePair<int,string>(6,"_NarrativeView"),
                new KeyValuePair<int,string>(7,"_DocumentsView"),
                new KeyValuePair<int,string>(8,"_MoneyView"),
                new KeyValuePair<int,string>(9,"_LogView")
            };
            ViewBag.SectionList = views.OrderBy(v => v.Key).Select(v => v.Value).ToList();
            return View();
        }
    }
}