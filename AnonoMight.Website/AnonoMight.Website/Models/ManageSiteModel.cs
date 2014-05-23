using AnonoMight.Model;
using AnonoMight.Website.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AnonoMight.Website.Models
{
	public sealed class ManageSiteModel
	{
		
		public ManageSiteModel()
		{

			
		}

		public ManageSiteModel(Site site)
		{
			if(site!=null)
			{

				Active = site.Active;
				Name = site.SiteName;
				Title = site.SiteTitle;
				Description = site.Description;
				SiteType = (SiteType)site.SiteType;
			}
		}

		[Required]
		[Display(Name = "Active?")]
		public bool Active { get; set; }

		[Required]
		[Display(Name = "Name")]
		public string Name { get; set; }

		[Required]
		[Display(Name = "Title")]
		public string Title { get; set; }


		[Display(Name = "Description")]
		public string Description { get; set; }

		[Display(Name = "Logo")]
		public byte[] LogoData { get; set; }

		[Display(Name = "Site Type")]
		public SiteType SiteType { get; set; }


		
	}
}