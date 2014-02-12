using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using ServiceHub.Website.Filters;
using ServiceHub.Website.Models;

namespace ServiceHub.Website.Controllers
{
	[Authorize]
	public class UserController : Controller
	{
		public ActionResult UserProfile()
		{
			
			return View();
		}

		

		
	}
}
