using AnonoMight.Model;
using AnonoMight.Website.Services;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AnonoMight.Website.Models
{
	public sealed class FacebookLinkModel
	{
		private Site currentSite;
		private System.Data.Entity.DbSet<FacebookProfile> dbSet;
		private FacebookProfile facebookProfile;
		private int page;

		public FacebookLinkModel(Site site, IQueryable<FacebookProfile> tweets, FacebookProfile topProfile, int page)
		{
			SiteInfo = new SiteInfoModel(site);
			ContentList = new PagedList<FacebookProfile>(tweets.OrderByDescending(x => x.TimeStamp), page, 10);
			TopProfile = topProfile;
		}

		public SiteInfoModel SiteInfo { get; set; }

		public FacebookProfile TopProfile { get; set; }


		public PagedList.IPagedList<AnonoMight.Model.FacebookProfile> ContentList { get; set; }
	}
}