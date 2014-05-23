using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServiceHub.Website.Controllers
{
	public class DirectoryController : BaseController
	{

		private readonly DirectoryService _directoryService;
		private readonly LookupService _lookupService;
		public DirectoryController(DirectoryService directoryService, LookupService lookupService)
		{

			_directoryService = directoryService;
			_lookupService = lookupService;
		}

		[HttpGet]
		public ActionResult ServiceProviders(int? page, string searchString, IEnumerable<int> locations, IEnumerable<Guid> tags)
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

			ViewBag.TagLookup = new MultiSelectList(_lookupService.GetTags(), "Id", "Value");
			ViewBag.LocationLookup = new MultiSelectList(_lookupService.GetLocations(), "Id", "Value");
			return View(_directoryService.GetServiceProvidersPage(pageNo, 10, locations, tags, searchString));
		}

		
		[HttpGet]
		public ActionResult Thumbnail(Guid userId)
		{
			byte[] imageBytes = null;
			imageBytes = _directoryService.GetLogoData(userId);

			if (imageBytes == null)
			{
				return new FilePathResult("~/Images/noimage.png", "image/png");
			}
			else
			{
				return new FileContentResult(imageBytes, "image/jpeg");
			}
		}

		[HttpGet]
		public ActionResult Services(
			int? page,
			string searchString,
			IEnumerable<int> locations,
			IEnumerable<Guid> tags,
			DateTime? beginBiddingCompletionDate,
			DateTime? endBiddingCompletionDate,
			DateTime? beginEstimatedServiceDate,
			DateTime? endEstimatedServiceDate
			)
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

			ViewBag.TagLookup = new SelectList(_lookupService.GetTags(), "Id", "Value");
			ViewBag.LocationLookup = new SelectList(_lookupService.GetLocations(), "Id", "Value");

			return View(_directoryService.GetServicesPage(pageNo, 10, locations, tags, searchString, beginBiddingCompletionDate, endBiddingCompletionDate, beginEstimatedServiceDate, endEstimatedServiceDate));
		}

		

	}
}
