using ServiceHub.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
namespace ServiceHub.Website.Routing
{
	public class MustHaveProfileRouteConstraint : IRouteConstraint
	{
		public MustHaveProfileRouteConstraint()
			{ }

			public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
			{
				string controller =(string)values["page"];
				string action =(string)values["subpage"];
				if (string.Equals(controller, "Client", StringComparison.OrdinalIgnoreCase) && !string.Equals(action, "MyServices", StringComparison.OrdinalIgnoreCase)
					|| string.Equals(controller, "ServiceProvider", StringComparison.OrdinalIgnoreCase) && !string.Equals(action, "MyBids", StringComparison.OrdinalIgnoreCase)
					)
				{
					string aspNetUserId = httpContext.User.Identity.GetUserId();
					bool hasProfile = DependencyResolver.Current.GetService<ServiceHubEntities>().Users.Any(o => o.AspNetUserId == aspNetUserId);
					return !hasProfile;
				}
				else
					return false;
			}
		
	}
}