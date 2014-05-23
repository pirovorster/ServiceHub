using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace ServiceHub.Website.Controllers
{
	public class ServiceProviderController : BaseController
	{

		private readonly UserProfileService _userProfileService;
		private readonly ServiceProviderService _serviceProviderService;
		private readonly LookupService _lookupService;
		public ServiceProviderController(LookupService lookupService, UserProfileService userProfileService, ServiceProviderService serviceProviderService)
		{
			_lookupService = lookupService;
			_serviceProviderService = serviceProviderService;
			_userProfileService = userProfileService;
		}
		//
		[Authorize]
		[HttpGet]
		public ActionResult ServiceBid(Guid serviceId)
		{
			return View(_serviceProviderService.GetServiceBid(serviceId));
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
		public ActionResult Bid(Guid serviceId, decimal bid)
		{
			_serviceProviderService.MakeBid(serviceId, bid);
			return Content(string.Format(CultureInfo.InvariantCulture,"{0}",bid));
		}

		[HttpPost]
		[Authorize]
		public ActionResult CancelBid(Guid serviceId)
		{
			throw new NotImplementedException();
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

			return View(_serviceProviderService.MyBidItems());
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult Thumbnail(Guid userId)
		{
			byte[] imageBytes = null;
			imageBytes = _userProfileService.GetLogoData(userId);

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
