using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SLS_LegalServices.Models;
using SLS_LegalServices.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SLS_LegalServices.Repositories
{
    public class RoleRepositoryImpl : IRoleRepository
    {
        private SLS_LegalServicesEntities context;
        private RoleManager<IdentityRole> roleManager;
        private UserManager<ApplicationUser> userManager;

        public RoleRepositoryImpl()
        {
            context = new SLS_LegalServicesEntities();
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }
        public void Delete(RoleViewModel roleViewModel)
        {
            IdentityRole role = roleManager.Roles.Where(r => r.Id == roleViewModel.Id).FirstOrDefault();
            roleManager.Delete(role);
        }

        public void DeleteRoleByUserName(string username)
        {

            List<AspNetRole> roles = context.AspNetRoles
                .Where(r => r.AspNetUsers.Any(u => u.UserName == username))
                .ToList();
            User user = context.Users.Where(u => u.UserName == username).FirstOrDefault();
            foreach (AspNetRole role in roles)
                userManager.RemoveFromRole(user.UserId.ToString(), role.Id);
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public List<RoleViewModel> GetAllRoles()
        {
            return context.AspNetRoles
                .Select(r => new RoleViewModel()
                    {
                        Id = r.Id,
                        Role = r.Name
                    }
                ).ToList();
        }
        

        public List<RoleViewModel> GetRolesByUsername(string username)
        {
            List<RoleViewModel> roles = context.AspNetRoles
                .Where(role => role.AspNetUsers.Any(user => user.UserName == username))
                .Select(r => new RoleViewModel()
                {
                    Id = r.Id,
                    Role = r.Name
                }
                ).ToList();
            return roles;
        }

        public void Insert(Guid id, string role)
        {
            CreateRolIfNotExist(role);
            ApplicationUser appUser = userManager.FindById(id.ToString());
            userManager.AddToRole(id.ToString(),role);
        }

        public void InsertMany(Guid id, List<RoleViewModel> roles)
        {
            if (roles != null)
            {
                foreach (RoleViewModel r in roles)
                    Insert(id, r.Role);
            }
            
        }

        private void CreateRolIfNotExist(string role)
        {
            if (!roleManager.RoleExists(role))
            {
                IdentityRole identityRole = new IdentityRole()
                {
                    Id = role,
                    Name = role
                };
                roleManager.Create(identityRole);
            }
        }
        public void Update(RoleViewModel roleViewModel)
        {
            throw new NotImplementedException();
        }
    }
}