using AnonoMight.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnonoMight.Website.Controllers
{
	public class HomeController : BaseController
	{
		public ActionResult Index()
		{
			using (AnonoMightEntities anonoMightEntities = new AnonoMightEntities())
			{
				IEnumerable<string> sites = anonoMightEntities.Sites.Where(o => o.Active).Select(o=>o.SiteName).ToList();
				return View(sites);
			}
			
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}