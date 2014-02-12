using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServiceHub.Website.Controllers
{
	public class ClientController : Controller
	{
		public ActionResult Services()
		{
			
			return View();
		}

		public ActionResult MyServices()
		{
			return View(Guid.NewGuid());
		} 
	}
}
