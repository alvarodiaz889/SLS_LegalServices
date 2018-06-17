using SLS_LegalServices.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SLS_LegalServices.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Basic_Usage_Submit(TestVM vm)
        {
            if (vm != null)
            {
                //var c = Basic_Usage_Get_File_Info(files);
                //TempData["UploadedFiles"] = c;
            }

            return Json(new { });
        }

        private IEnumerable<string> Basic_Usage_Get_File_Info(IEnumerable<HttpPostedFileBase> files)
        {
            return
                from a in files
                where a != null
                select string.Format("{0} ({1} bytes)", Path.GetFileName(a.FileName), a.ContentLength);
        }
    }
}