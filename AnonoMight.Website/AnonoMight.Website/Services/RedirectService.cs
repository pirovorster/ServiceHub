using AnonoMight.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnonoMight.Website.Services
{
	internal class RedirectService
	{
		internal string GetController(string siteName)
		{
			using (AnonoMightEntities anonoMightEntities = new AnonoMightEntities())
			{
				Site site = anonoMightEntities.Sites.SingleOrDefault(o => o.SiteName == siteName);
				if (site == null)
					return null;
				return ((SiteType)site.SiteType).ToString();

			}

		}
	}
}