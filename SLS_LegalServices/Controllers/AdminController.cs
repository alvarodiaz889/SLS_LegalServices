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

        public AdminController(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
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
            return View();
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