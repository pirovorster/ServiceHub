using ServiceHub.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ServiceHub.Website.Controllers
{

	[Authorize]
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
		[HttpGet]
		public ActionResult ServiceBid(Guid serviceId)
		{
			return View(_serviceProviderService.GetServiceBid(serviceId));
		}

		[HttpPost]
		public ActionResult Bid(Guid serviceId, decimal bid)
		{
			_serviceProviderService.MakeBid(serviceId, bid);
			return Content(string.Format(CultureInfo.InvariantCulture, "{0}", bid));
		}

		[HttpPost]
		public ActionResult CancelBid(Guid serviceId)
		{
			throw new NotImplementedException();
		}



		[HttpPost]
		public ActionResult AddAdditionalInfoRequest(Guid serviceId, string additionalInfo)
		{

			_serviceProviderService.RequestAdditionalInfo(serviceId, additionalInfo);
			return Content("Request has been sent.");
		}

		[HttpGet]
		public ActionResult MyBids()
		{

			return View(_serviceProviderService.MyBidItems());
		}

		private void SetViewBagData()
		{
			ViewBag.TagLookup = new MultiSelectList(_lookupService.GetTags(), "Id", "Value");
			ViewBag.LocationLookup = new MultiSelectList(_lookupService.GetLocations(), "Id", "Value");
		}

		

	}
}
