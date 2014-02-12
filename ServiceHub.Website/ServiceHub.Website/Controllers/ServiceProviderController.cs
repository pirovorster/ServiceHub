using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServiceHub.Website.Controllers
{
	public class ServiceProviderController : Controller
	{
		public ActionResult List()
		{
			
			return View();
		}

		public ActionResult MyBids()
		{

			return View(Guid.NewGuid());
		} 
	}
}
