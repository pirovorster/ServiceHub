using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ServiceHub.Website.Startup))]
namespace ServiceHub.Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
