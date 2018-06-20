using SLS_LegalServices.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLS_LegalServices.Repositories
{
    public interface IUserRepository
    {
        void Insert(UserViewModel userViewModel);
        void InsertApplicationUser(string id, string username);
        void Update(UserViewModel userViewModel);
        void UpdateApplicationUser(string id, string username);
        void Delete(UserViewModel userViewModel);
        void DeleteApplicationUser(string id);
        List<UserViewModel> GetAllUsers();
        UserViewModel GetUserByUserName(string userName);
        void Dispose();

    }
}
