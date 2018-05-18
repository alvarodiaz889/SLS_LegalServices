
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
        private static Func<User, bool> isNotSuperAdmin = IsNotSuperAdmin;

        public UserRepositoryImpl() : this(new RoleRepositoryImpl()){ }
        public UserRepositoryImpl(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
            context = new SLS_LegalServicesEntities();
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }

        public List<UserViewModel> GetAllUsers()
        {
            List<UserViewModel> users = context.Users
                .Where(isNotSuperAdmin)
                .Select(u => new UserViewModel()
                {
                    UserName = u.US_Username,
                    FirstName = u.US_FirstName,
                    MiddleName = u.US_MiddleName,
                    LastName = u.US_LastName,
                    Email = u.US_Email,
                }).ToList();
                users.ForEach(u => {
                    u.Roles = roleRepository.GetRolesByUsername(u.UserName).ToList();
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
            User user = context.Users.Where(u => u.US_Username == userViewModel.UserName).FirstOrDefault();
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
            ApplicationUser user = new ApplicationUser { UserName = userViewModel.UserName, Email = userViewModel.Email };
            userManager.Create(user);
            return user.Id;
        }
        private int InsertUser(UserViewModel userViewModel,string Id)
        {
            User user = new User()
            {
                US_Id = Guid.Parse(Id),
                US_FirstName = userViewModel.FirstName,
                US_LastName = userViewModel.LastName,
                US_MiddleName = userViewModel.MiddleName,
                US_Email = userViewModel.Email,
                US_Username = userViewModel.UserName
            };
            context.Users.Add(user);
            return context.SaveChanges();
        }
        private void InsertRolesForUser(UserViewModel userViewModel)
        {
            User user = context.Users.Where(u => u.US_Username == userViewModel.UserName).FirstOrDefault();
            roleRepository.InsertMany(user.US_Id, userViewModel.Roles);
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
            User user = context.Users.Where(u => u.US_Username == userViewModel.UserName).FirstOrDefault();
            if (user != null)
            {
                user.US_FirstName = userViewModel.FirstName;
                user.US_LastName = userViewModel.LastName;
                user.US_MiddleName = userViewModel.MiddleName;
                user.US_Email = userViewModel.Email;
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
            UserViewModel user = context.Users.Where(u => u.US_Username == userName)
                .Select(u => new UserViewModel()
                {
                    UserName = u.US_Username,
                    FirstName = u.US_FirstName,
                    MiddleName = u.US_MiddleName,
                    LastName = u.US_LastName,   
                    Email = u.US_Email,
                }).FirstOrDefault();
            if (user != null)
            {
                user.Roles = roleRepository.GetRolesByUsername(user.UserName).ToList();
                user.UserRoles = string.Join(",", user.Roles.Select(c => c.Role));
            }
            
            return user;
        }
        public void Dispose()
        {
            context.Dispose();
        }

        private static bool IsNotSuperAdmin(User user)
        {
            bool answer = true;
            if (user?.US_IsSuperAdmin != null)
                if ((bool)user?.US_IsSuperAdmin)
                    answer = false;
            return answer;
        }
        
    }
}