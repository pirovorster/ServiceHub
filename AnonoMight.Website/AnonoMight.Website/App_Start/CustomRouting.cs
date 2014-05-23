using AnonoMight.Website.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AnonoMight.Website
{
	public class CustomRouting : MvcRouteHandler
	{
		protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
		{
			var site = requestContext.HttpContext.Request.Path.TrimStart('/');

			if (!string.IsNullOrEmpty(site))
			{
				string[] tokens =site.Split('/');
				string siteName = tokens.First();
				string controller = new RedirectService().GetController(siteName);
				if (controller != null)
				{
					FillRequest(controller,
						tokens.Length==2? tokens.Last():"Content",
						siteName,
						requestContext);
				}
			}

			return base.GetHttpHandler(requestContext);
		}

		private static void FillRequest(string controller, string action,string site,RequestContext requestContext)
		{
			if (requestContext == null)
			{
				throw new ArgumentNullException("requestContext");
			}

			requestContext.RouteData.Values["controller"] = controller;
			requestContext.RouteData.Values["action"] = action;
			requestContext.RouteData.Values["site"] = site;
		}
	}
}