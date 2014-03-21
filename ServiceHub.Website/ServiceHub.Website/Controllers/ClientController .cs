using ServiceHub.Model;
using ServiceHub.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace ServiceHub.Website.Controllers
{
	public class ClientController : BaseController
	{
		private readonly LookupService _lookupService;
		private readonly ServiceService _clientService;
		public ClientController(LookupService lookupService, ServiceService clientService)
		{
			_lookupService = lookupService;
			_clientService = clientService;
		}
		public ActionResult MyServices()
		{
			return View(_clientService.MyServiceItems());
		}

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

			SetViewBagServiceListData();
			return View(_clientService.GetServicesPage(pageNo, 10, locations, tags, searchString, beginBiddingCompletionDate, endBiddingCompletionDate, beginEstimatedServiceDate, endEstimatedServiceDate));
		}

		private void SetViewBagServiceListData()
		{
			ViewBag.TagLookup = new MultiSelectList(_lookupService.GetTags(), "Id", "Value");
			ViewBag.LocationLookup = new MultiSelectList(_lookupService.GetLocations(), "Id", "Value");
		}

		[HttpGet]
		public ActionResult PostService()
		{
			SetViewBagPostServiceData();
			return View();
		}

		[HttpPost]
		public ActionResult PostService(PostServiceViewModel postServiceViewModel)
		{
			if (ModelState.IsValid)
			{
				_clientService.PostService(postServiceViewModel);

				SetViewBagPostServiceData();
				return View(postServiceViewModel);
			}

			SetViewBagPostServiceData();
			return View(postServiceViewModel);
		}


		private void SetViewBagPostServiceData()
		{
			ViewBag.TagLookup = new SelectList(_lookupService.GetTags(), "Id", "Value");
			ViewBag.LocationLookup = new SelectList(_lookupService.GetLocations(), "Id", "Value");
		}


	}
}
