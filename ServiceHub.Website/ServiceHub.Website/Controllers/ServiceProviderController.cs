using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace ServiceHub.Website.Controllers
{
	public class ServiceProviderController : Controller
	{
		private readonly ServiceProviderService _service;
		public ServiceProviderController()
		{

			_service = new ServiceProviderService();
		}

		public ActionResult List(int? page)
		{

			return View(_service.GetServiceProviderPage(page.HasValue?page.Value:1,10));
		}

		public ActionResult MyBids()
		{

			return View(Guid.NewGuid());
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult Thumbnail(Guid serviceProviderId)
		{
			byte[] imageBytes = null;
			imageBytes =  _service.GetLogoData(serviceProviderId);

			if (imageBytes == null)
			{
				return new FilePathResult("~/Images/noimage.png", "image/png");
			}
			else
			{
				return new FileContentResult(imageBytes, "image/jpeg");
			}
		}

	}
}
