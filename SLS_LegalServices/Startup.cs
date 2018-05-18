using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SLS_LegalServices.Startup))]
namespace SLS_LegalServices
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
