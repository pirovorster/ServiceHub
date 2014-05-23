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
	public abstract class BaseSiteController : BaseController
	{
		protected AnonoMightEntities _anonoMightEntities;

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{

			_anonoMightEntities.SaveChanges();
			_anonoMightEntities.Dispose();
			base.OnActionExecuted(filterContext);
		}
		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			string siteName = (string)filterContext.RouteData.Values["site"];

			_anonoMightEntities = ContextFactory.GetContext(siteName);

			base.OnActionExecuting(filterContext);
		}


		internal abstract IContextFactory ContextFactory { get; }
	}
}