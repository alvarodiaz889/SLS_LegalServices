using SLS_LegalServices.Repositories;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace SLS_LegalServices
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IRoleRepository, RoleRepositoryImpl>();
            container.RegisterType<IUserRepository, UserRepositoryImpl>();
            container.RegisterType<IMainRepository, MainRepository>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}