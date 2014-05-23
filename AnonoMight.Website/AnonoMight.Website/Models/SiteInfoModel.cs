using AnonoMight.Model;
using AnonoMight.Website.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AnonoMight.Website.Models
{
	public sealed class SiteInfoModel
	{
		
		private readonly string _name;
		private readonly string _title;
		private readonly string _description;

	
		public SiteInfoModel(Site site)
		{
			if(site!=null)
			{
				_name = site.SiteName;
				_title = site.SiteTitle;
				_description = site.Description;
			}
		}


		public string Name { get { return _name; } }
		public string Title { get { return _title; } }
		public string Description { get { return _description; } }


		
	}
}