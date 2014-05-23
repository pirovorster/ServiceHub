using AnonoMight.Model;
using AnonoMight.Website.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnonoMight.Website.Controllers
{
	public abstract class BaseController : Controller
	{
		
		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{

			using (AnonoMightEntities anonoMightEntities = new AnonoMightEntities())
			{
				ViewBag.Sites = anonoMightEntities.Sites.Where(o => o.Active).Select(o=>new {SiteName=o.SiteName, SiteTitle = o.SiteTitle}).ToList().Select(o => new KeyValuePair<string, string>(o.SiteName, o.SiteTitle));
			}

			base.OnActionExecuted(filterContext);
		}
		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
		
		}


	}
}