using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AnonoMight.Website
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			
			routes.MapRoute(
				"Account",
				"Account/{action}",
				new { controller = "Account", action = "Index", site = UrlParameter.Optional }
				);

			routes.MapRoute(
				"Site",
				"Site/{action}/{siteName}",
				new { controller = "Site", action = "Index", siteName = "Unknown" }
				);

			routes.MapRoute(
				"Default",
				"Home/{action}",
				new { controller = "Home", action = "Index", site = UrlParameter.Optional }
				);

			
			
			routes.MapRoute(
				"Custom1",
				"{site}",
				new { controller = "Home", action = "Index", site = UrlParameter.Optional }
				).RouteHandler = new CustomRouting();

			routes.MapRoute(
				"Custom2",
				"{site}/{subsite}",
				new { controller = "Home", action = "Index", site = UrlParameter.Optional }
				).RouteHandler = new CustomRouting();

			
		}
	}
}
