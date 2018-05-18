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
        void Update(UserViewModel userViewModel);
        void Delete(UserViewModel userViewModel);
        List<UserViewModel> GetAllUsers();
        UserViewModel GetUserByUserName(string userName);
        void Dispose();

    }
}
