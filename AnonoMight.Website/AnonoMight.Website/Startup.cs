using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AnonoMight.Website.Startup))]
namespace AnonoMight.Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
			app.MapSignalR();
        }
    }
}
