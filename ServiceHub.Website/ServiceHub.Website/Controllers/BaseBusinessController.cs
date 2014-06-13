using ServiceHub.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.Routing;
using System.IO;
namespace ServiceHub.Website.Controllers
{

	public class BaseBusinessController : BaseController
    {
		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			
			DependencyResolver.Current.GetService<ServiceHubEntities>().SaveChanges();
			base.OnActionExecuted(filterContext);
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			
			if (string.Equals(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,"Client") &&!string.Equals(filterContext.ActionDescriptor.ActionName, "MyServices", StringComparison.OrdinalIgnoreCase)
				|| string.Equals(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, "ServiceProvider") && !string.Equals(filterContext.ActionDescriptor.ActionName, "MyBids", StringComparison.OrdinalIgnoreCase))
			{
				ServiceHubEntities serviceHubEntities = DependencyResolver.Current.GetService<ServiceHubEntities>();
				string aspNetUserId = User.Identity.GetUserId();
				bool hasProfile = DependencyResolver.Current.GetService<ServiceHubEntities>().Users.Any(o => o.AspNetUserId == aspNetUserId);

				if (!hasProfile)
				{

					filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary 
					{
						{ "Controller", "User" },
                        { "Action", "UserProfile" } 
					});
					
				}


			}
			base.OnActionExecuting(filterContext);
		}

    }
}
