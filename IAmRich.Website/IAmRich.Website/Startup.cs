using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IAmRich.Website.Startup))]
namespace IAmRich.Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
