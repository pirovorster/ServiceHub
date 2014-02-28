using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace ServiceHub.Website.Controllers
{
	public class ServiceProviderController : Controller
	{

		private readonly ClientService _clientService;
		private readonly ServiceProviderService _serviceProviderService;
		private readonly LookupService _lookupService;
		public ServiceProviderController()
		{
			_lookupService = new LookupService();
			_serviceProviderService = new ServiceProviderService();
			_clientService = new ClientService();
		}
		//
		[Authorize]
		[HttpGet]
		public ActionResult ServiceBid(Guid serviceId)
		{
			return View(_clientService.GetService(serviceId));
		}

		public ActionResult List(int? page, string searchString,IEnumerable<int> locations, IEnumerable<Guid> tags)
		{
			int pageNo = 1;

			if (page.HasValue)
				pageNo = page.Value;

			if (locations == null)
				locations = Enumerable.Empty<int>();

			if (tags == null)
				tags = Enumerable.Empty<Guid>();

			if (string.IsNullOrWhiteSpace(searchString))
				searchString = string.Empty;

			ViewBag.CurrentSearchString = searchString;

			ViewBag.CurrentLocations = locations;

			ViewBag.CurrentTags = tags;

			SetViewBagData();
			return View(_serviceProviderService.GetServiceProvidersPage(pageNo, 10, locations, tags, searchString));
		}

		[HttpPost]
		[Authorize]
		public ActionResult Bid(Guid serviceId,Guid serviceProviderId, decimal bid)
		{
			_serviceProviderService.Bid(serviceId, serviceProviderId, bid);
			return Content(string.Format(CultureInfo.InvariantCulture,"{0}",bid));
		}

		[HttpPost]
		[Authorize]
		public ActionResult AddAdditionalInfoRequest(Guid serviceId, Guid serviceProviderId, string additionalInfo)
		{

			_serviceProviderService.RequestAdditionalInfo(serviceId, serviceProviderId, additionalInfo);
			return Content("Request has been sent.");
		}
		
		
		public ActionResult MyBids()
		{

			return View(Guid.NewGuid());
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult Thumbnail(Guid serviceProviderId)
		{
			byte[] imageBytes = null;
			imageBytes =  _serviceProviderService.GetLogoData(serviceProviderId);

			if (imageBytes == null)
			{
				return new FilePathResult("~/Images/noimage.png", "image/png");
			}
			else
			{
				return new FileContentResult(imageBytes, "image/jpeg");
			}
		}
		private void SetViewBagData()
		{
			ViewBag.TagLookup = new MultiSelectList(_lookupService.GetTags(), "Id", "Value");
			ViewBag.LocationLookup = new MultiSelectList(_lookupService.GetLocations(), "Id", "Value");
		}

	}
}
