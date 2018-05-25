
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SLS_LegalServices.Models;
using SLS_LegalServices.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SLS_LegalServices;

namespace SLS_LegalServices.Repositories
{
    public class UserRepositoryImpl : IUserRepository
    {
        private SLS_LegalServicesEntities context;
        private UserManager<ApplicationUser> userManager;
        private IRoleRepository roleRepository;

        public UserRepositoryImpl() : this(new RoleRepositoryImpl()){ }
        public UserRepositoryImpl(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
            context = new SLS_LegalServicesEntities();
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }

        public List<UserViewModel> GetAllUsers()
        {
            var query = from u in context.Users
                        where !(from a in context.Attorneys
                                select a.UserId)
                        .Contains(u.UserId)
                        select new UserViewModel {
                            UserId = u.UserId,
                            UserName = u.UserName,
                            FirstName = u.FirstName,
                            Active = u.Active,
                            DisplayName = u.DisplayName,
                            LastName = u.LastName
                        };
            List<UserViewModel> users = query.ToList();
            users.ForEach(u => {
                u.Roles = roleRepository.GetRolesByUserId(u.UserId.ToString()).ToList();
                u.UserRoles = string.Join(",",u.Roles.Select(c => c.Role));
            });
            
            return users;
        }

        #region Delete
        public void Delete(UserViewModel userViewModel)
        {
            DeleteUser(userViewModel);
            DeleteApplicationUser(userViewModel);
        }

        private void DeleteRoleByUserName(string username)
        {
            roleRepository.DeleteRoleByUserName(username);
        }
        private void DeleteUser(UserViewModel userViewModel)
        {
            User user = context.Users.Where(u => u.UserName == userViewModel.UserName).FirstOrDefault();
            if (user == null)
                throw new Exception("User does not exist.");
            context.Users.Remove(user);
            context.SaveChanges();
        }
        private void DeleteApplicationUser(UserViewModel userViewModel)
        {
            ApplicationUser appUser = userManager.Users.Where(w => w.UserName == userViewModel.UserName).FirstOrDefault();
            if (appUser == null)
                throw new Exception("User does not exist.");
            userManager.Delete(appUser);
        }
        #endregion


        #region Insert
        public void Insert(UserViewModel userViewModel)
        {
            string appUserId = InsertApplicationUser(userViewModel);
            int rows = InsertUser(userViewModel,appUserId);
            if (rows > 0)
                InsertRolesForUser(userViewModel);
            else
                DeleteApplicationUser(userViewModel);

        }
        private string InsertApplicationUser(UserViewModel userViewModel)
        {
            ApplicationUser user = new ApplicationUser { UserName = userViewModel.UserName};
            userManager.Create(user);
            return user.Id;
        }
        private int InsertUser(UserViewModel userViewModel,string Id)
        {
            User user = new User()
            {
                UserId = Guid.Parse(Id),
                FirstName = userViewModel.FirstName,
                LastName = userViewModel.LastName,
                Active = userViewModel.Active,
                DisplayName = userViewModel.DisplayName,
                UserName = userViewModel.UserName
            };
            context.Users.Add(user);
            return context.SaveChanges();
        }
        private void InsertRolesForUser(UserViewModel userViewModel)
        {
            User user = context.Users.Where(u => u.UserName == userViewModel.UserName).FirstOrDefault();
            roleRepository.InsertMany(user.UserId, userViewModel.Roles);
        }
        #endregion

        #region Update
        public void Update(UserViewModel userViewModel)
        {
            int rowsAffected = UpdateUser(userViewModel);
            UpdateRoles(userViewModel);
        }
        private int UpdateUser(UserViewModel userViewModel)
        {
            int rowsAffected = 0;
            User user = context.Users.Where(u => u.UserName == userViewModel.UserName).FirstOrDefault();
            if (user != null)
            {
                user.FirstName = userViewModel.FirstName;
                user.LastName = userViewModel.LastName;
                user.Active = userViewModel.Active;
                user.DisplayName = userViewModel.DisplayName;
                rowsAffected = context.SaveChanges();
            }
            return rowsAffected;
        }
        private void UpdateRoles(UserViewModel userViewModel)
        {
            DeleteRoleByUserName(userViewModel.UserName);
            InsertRolesForUser(userViewModel);
        }
        #endregion

        public UserViewModel GetUserByUserName(string userName)
        {
            UserViewModel user = context.Users.Where(u => u.UserName == userName)
                .Select(u => new UserViewModel()
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    Active = u.Active,
                    DisplayName = u.DisplayName,
                    LastName = u.LastName
                }).FirstOrDefault();
            if (user != null)
            {
                user.Roles = roleRepository.GetRolesByUserId(user.UserId.ToString()).ToList();
                user.UserRoles = string.Join(",", user.Roles.Select(c => c.Role));
            }
            
            return user;
        }
        public void Dispose()
        {
            context.Dispose();
        }
        
    }
}