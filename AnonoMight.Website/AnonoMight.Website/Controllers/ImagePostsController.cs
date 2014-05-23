using AnonoMight.Model;
using AnonoMight.Website.Models;
using AnonoMight.Website.Services;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AnonoMight.Website.Controllers
{
	public class ImagePostsController : BaseSiteController
	{

		private readonly Guid _secret = Guid.Parse("0d2da649-af58-4eae-90a2-7c452478b0e1");
		private readonly DataPersistor _dataPersistor = new DataPersistor();

		public ActionResult Content(string site, int page = 1)
		{
			using (AnonoMightEntities anonoMightEntities = ContextFactory.GetContext(site))
			{
				Site currentSite = anonoMightEntities.Sites.SingleOrDefault(o => o.SiteName == site && o.Active);

				if (currentSite != null)
				{
					return View(new ImagePostModel(currentSite, anonoMightEntities.ImagePosts, page));
				}
				else
					return Content("This site does not exist!");
			}
		}


		

		public async Task<ActionResult> Upload(string site)
		{
			for (int i = 0; i < Request.Files.Count; i++)
			{
				var file = Request.Files[i];
				Guid id = Guid.NewGuid();


				byte[] logo = new byte[file.ContentLength];
				file.InputStream.Read(logo, 0, file.ContentLength);


				_dataPersistor.SaveImage(logo, site, id);
				var hubConnection = new HubConnection("http://localhost:4785/");
				IHubProxy stockTickerHubProxy = hubConnection.CreateHubProxy("NotificationHub");
				await hubConnection.Start();
				await stockTickerHubProxy.Invoke("NotifyPostedImage", new NotifyPostedImageCommand { Id = id, Secret = _secret, Image = logo, Timestamp = DateTime.Now, SiteName = (string)RouteData.Values["site"] });
				hubConnection.Stop();
			}
			return Json(new { success = true }, JsonRequestBehavior.AllowGet);
		}

		internal override IContextFactory ContextFactory
		{
			get { return new ImagePostsContextFactory(); }
		}
	}
}