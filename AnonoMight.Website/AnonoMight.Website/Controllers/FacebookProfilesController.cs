using AnonoMight.Model;
using AnonoMight.Website.Models;
using AnonoMight.Website.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnonoMight.Website.Controllers
{
	public class FacebookProfilesController : BaseSiteController
    {
		public ActionResult Content(string site, int page = 1)
        {
			FacebookProfile facebookProfile =null;
			using (AnonoMightEntities anonoMightEntities = new AnonoMightEntities())
			{
				facebookProfile=  anonoMightEntities.GetTopFacebookLinkForTheLastMonth(site).SingleOrDefault();
			}

			using (AnonoMightEntities anonoMightEntities = ContextFactory.GetContext(site))
			{
				Site currentSite = anonoMightEntities.Sites.SingleOrDefault(o => o.SiteName == site && o.Active);

				if (currentSite != null)
				{
					return View(new FacebookLinkModel(currentSite, anonoMightEntities.FacebookProfiles, facebookProfile, page));
				}
				else
					return Content("This site does not exist!");
			}


        }

		internal override IContextFactory ContextFactory
		{
			get { return new FacebookProfilesContextFactory(); }
		}
	}
}