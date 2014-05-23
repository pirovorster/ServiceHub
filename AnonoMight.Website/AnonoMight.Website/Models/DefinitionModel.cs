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
	public sealed class DefinitionModel
	{
		public DefinitionModel(Site site, IQueryable<Definition> tweets, int page)
		{
			SiteInfo = new SiteInfoModel(site);
			ContentList = new PagedList<Definition>(tweets.OrderByDescending(x => x.TimeStamp), page, 10);
			
		}

		public SiteInfoModel SiteInfo { get; set; }

		public PagedList.IPagedList<AnonoMight.Model.Definition> ContentList { get; set; }
	}
}