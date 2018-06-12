using SLS_LegalServices.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SLS_LegalServices.Controllers
{
    public class AdminController : Controller
    {

        private IUserRepository userRepository;
        private IRoleRepository roleRepository;
        private IMainRepository mainRepository;

        public AdminController(IUserRepository userRepository, IRoleRepository roleRepository, IMainRepository mainRepository)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.mainRepository = mainRepository;
        }
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
            ViewBag.DateName = "CertifiedDate";
            ViewBag.Attorneys = mainRepository.GetAllAttorneys();
            return PartialView("_Interns");
        }

        public ActionResult CaseTypes()
        {
            return PartialView("_CaseTypes");
        }

        public ActionResult SystemUsers()
        {
            ViewBag.Roles = roleRepository.GetAllRoles();
            return PartialView("_SystemUsers");
        }

        public ActionResult CaseLog()
        {
            return PartialView("_CaseLog");
        }
    }
}