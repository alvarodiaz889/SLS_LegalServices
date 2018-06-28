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
    //[Authorize]
    public class UserController : Controller
    {
        private IUserRepository userRepository;
        private IRoleRepository roleRepository;

        public UserController(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
        }

        public JsonResult Read([DataSourceRequest]DataSourceRequest request)
        {
            List<UserViewModel> users = userRepository.GetAllUsers();
            DataSourceResult result = users.AsQueryable().ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, UserViewModel uvm, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                userRepository.Insert(uvm);
            }

            return Json(new[] { uvm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, UserViewModel uvm, FormCollection form)
        {
            userRepository.Delete(uvm);
            return Json(new[] { uvm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, UserViewModel uvm, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                userRepository.Update(uvm);
            }
            return Json(new[] { uvm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult GetUserByUserName(string userName, string userId)
        {
            bool function(User u) => u.UserName == userName && u.UserId.ToString() != userId;
            var users = userRepository.GetUsersBy(function);
            return Json(users.Count() == 0);
        }

        protected override void Dispose(bool disposing)
        {
            userRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}