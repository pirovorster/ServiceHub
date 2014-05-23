using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace ServiceHub.Website
{
	public partial class Startup
	{
		// For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
		public void ConfigureAuth(IAppBuilder app)
		{
			// Enable the application to use a cookie to store information for the signed in user
			app.UseCookieAuthentication(new CookieAuthenticationOptions
			{
				AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
				LoginPath = new PathString("/Account/Login")
			});
			// Use a cookie to temporarily store information about a user logging in with a third party login provider
			app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

			// Uncomment the following lines to enable logging in with third party login providers
			//app.UseMicrosoftAccountAuthentication(
			//	clientId: "",
			//	clientSecret: "");

			app.UseTwitterAuthentication( consumerKey: "vAmDUWJX1AMBgokwjllOdR5l5 ",consumerSecret: "EIAckdhha1PcKWD6UtubvDULZr4c1Ii1ugldm24X2ATu6d2l01");

			app.UseFacebookAuthentication( appId: "489374234501612",appSecret: "672aebbf6ec37823fa9e2163bd0832e9");

			app.UseGoogleAuthentication();



		}
	}
}