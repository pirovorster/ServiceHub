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

	public class SiteController : BaseController
	{
		[HttpGet]
		[Authorize]
		public ActionResult Manage(string siteName)
		{
			using (AnonoMightEntities anonoMightEntities = new AnonoMightEntities())
			{
				Site site = anonoMightEntities.Sites.SingleOrDefault(o => o.SiteName == siteName);

				ManageSiteModel manageSiteModel = new ManageSiteModel(site);
				manageSiteModel.Name = siteName;
				ViewBag.Message = string.Empty;

				return View(manageSiteModel);
			}
		}

		[HttpPost]
		[Authorize]
		public ActionResult Manage(HttpPostedFileBase imageFile, ManageSiteModel manageSiteModel)
		{

			if (ModelState.IsValid)
			{
				if (imageFile != null)
				{
					byte[] logo = new byte[imageFile.ContentLength];
					imageFile.InputStream.Read(logo, 0, imageFile.ContentLength);
					using (AnonoMightEntities anonoMightEntities = new AnonoMightEntities())
					{
						Site site = anonoMightEntities.Sites.SingleOrDefault(o => o.SiteName == manageSiteModel.Name);


						if (site == null)
						{
							site = new Site();
							anonoMightEntities.Sites.Add(site);
							site.SiteName = manageSiteModel.Name;

							if (manageSiteModel.SiteType == SiteType.Definitions)
								anonoMightEntities.CreateDefinitionsTable(site.SiteName);
							else if (manageSiteModel.SiteType == SiteType.FacebookProfiles)
								anonoMightEntities.CreateFacebookProfilesTable(site.SiteName);
							else if (manageSiteModel.SiteType == SiteType.ImagePosts)
								anonoMightEntities.CreateImagePostsTable(site.SiteName);
							else if (manageSiteModel.SiteType == SiteType.Tweets)
								anonoMightEntities.CreateTweetsTable(site.SiteName);
						}

						site.SiteLogo = logo;
						site.SiteTitle = manageSiteModel.Title;
						site.SiteType = (byte)manageSiteModel.SiteType;
						site.Active = manageSiteModel.Active;
						site.Description = manageSiteModel.Description;



						anonoMightEntities.SaveChanges();
					}

					ViewBag.Message = "Site has been saved!";

				}
				else
				{
					ViewBag.Message = "No Image Selected!";

				}

			}

			return View(manageSiteModel);
		}





		[HttpGet]

		public ActionResult GetSiteImage(string siteName)
		{
			byte[] image = null;
			using (AnonoMightEntities anonoMightEntities = new AnonoMightEntities())
			{
				Site site = anonoMightEntities.Sites.SingleOrDefault(o => o.SiteName == siteName);
				if (site != null)
				{
					image = site.SiteLogo;
				}
			}

			if (image == null)
			{
				return new FilePathResult("~/Images/noimage.png", "image/png");
			}
			else
			{
				return new FileContentResult(image, "image/jpeg");
			}
		}

		[HttpPost]
		public ActionResult Report(string siteName, Guid postId, ReportClass reportClass)
		{
			using (AnonoMightEntities anonoMightEntities = new AnonoMightEntities())
			{
				anonoMightEntities.Reports.Add(new Report { PostId = postId, ReportClass = (byte)reportClass, SiteName = siteName, TimeStamp = DateTime.Now });
				anonoMightEntities.SaveChanges();


			}

			return Json("Success");
		}



	}
}