using ServiceHub.Model;
using ServiceHub.Website.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServiceHub.Website.Controllers
{
	
    public class BaseController : Controller
    {
		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			DependencyResolver.Current.GetService<ServiceHubEntities>().SaveChanges();
			base.OnActionExecuted(filterContext);
		}

    }
}
