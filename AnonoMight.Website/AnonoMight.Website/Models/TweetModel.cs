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
	public sealed class TweetModel
	{
		public TweetModel(Site site, IQueryable<Tweet> tweets,int page)
		{

			SiteInfo = new SiteInfoModel(site);
			ContentList = new PagedList<Tweet>(tweets.OrderByDescending(x => x.TimeStamp), page, 10);
			
		}

		public SiteInfoModel SiteInfo { get; set; }

		public PagedList.IPagedList<AnonoMight.Model.Tweet> ContentList { get; set; }
	}
}