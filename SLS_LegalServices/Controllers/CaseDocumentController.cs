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
    public class CaseDocumentController : Controller
    {
        IMainRepository repository;
        const string sessionKey = "RESUMEFILE";

        public CaseDocumentController(IMainRepository repository)
        {
            this.repository = repository;
        }
        public ActionResult Read([DataSourceRequest]DataSourceRequest request, int intakeId)
        {
            var notes = repository.GetAllCaseDocuments().Where(e => e.CaseId == intakeId).ToList();
            DataSourceResult result = notes.AsQueryable().ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, CaseDocumentVM obj)
        {
            if (ModelState.IsValid)
            {
                if (HttpContext.Session[sessionKey] != null)
                {
                    //saving the file
                    var file = HttpContext.Session[sessionKey] as HttpPostedFileBase;
                    var route = Server.MapPath("/App_Data/") + file.FileName;
                    file.SaveAs(route);
                    HttpContext.Session[sessionKey] = null;

                    //saving the record
                    obj.Filename = file.FileName;
                    obj.FileType = file.ContentType;
                    repository.CaseDocumentInsert(obj);
                }
                
            }

            return Json(new[] { obj }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, CaseDocumentVM obj)
        {
            var fileName = Server.MapPath("/App_Data/") + obj.Filename;
            if (System.IO.File.Exists(fileName))
                System.IO.File.Delete(fileName);
            repository.CaseDocumentDelete(obj);
            return Json(new[] { obj }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, CaseDocumentVM obj)
        {
            if (ModelState.IsValid)
            {
                if (HttpContext.Session[sessionKey] != null)
                {
                    //Edit not implemented
                }
            }
            return Json(new[] { obj }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult SaveResumeFile()
        {
            string filename = String.Empty;
            if (HttpContext.Request.Files != null && HttpContext.Request.Files.Count > 0 && HttpContext.Session != null)
            {
                HttpContext.Session[sessionKey] = HttpContext.Request.Files[0];
            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteResumeFile(string fileName)
        {
            HttpContext.Session[sessionKey] = null;
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFile(string name, string intakeId)
        {
            var path = Server.MapPath("/App_Data/") + name;
            var doc = repository.GetAllCaseDocuments().Where(d => d.Filename == name).FirstOrDefault();
            if (doc != null)
            {
                return File(path, doc.FileType,name);
            }
            return RedirectToAction("Details","Intakes",new { id = intakeId });
        }

        protected override void Dispose(bool disposing)
        {
            repository.Dispose();
            base.Dispose(disposing);
        }
    }
}