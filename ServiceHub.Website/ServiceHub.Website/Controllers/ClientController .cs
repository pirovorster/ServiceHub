using ServiceHub.Model;
using ServiceHub.Website.Models;
using ServiceHub.Website.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ServiceHub.Website.Controllers
{
	[Authorize]
	public class ClientController : BaseController
	{
		private readonly LookupService _lookupService;
		private readonly ClientService _clientService;

		public ClientController(LookupService lookupService, ClientService clientService)
		{
			_lookupService = lookupService;
			_clientService = clientService;
		}

		[HttpGet]
		public ActionResult MyServices()
		{

			return View(_clientService.MyServiceItems());
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

		[HttpPost]
		public ActionResult CancelSevice(Guid serviceId)
		{
			_clientService.CancelService(serviceId);
			return View("Service", _clientService.GetService(serviceId));
		}

		[HttpGet]
		public ActionResult Service(Guid serviceId)
		{

			return View(_clientService.GetService(serviceId));
		}


		[HttpGet]
		public ActionResult BidAcceptance(Guid serviceId)
		{
			return View(_clientService.GetBidsToAccept(serviceId));
		}


		[HttpPost]
		public ActionResult AcceptService(Guid userId, Guid serviceId)
		{

			_clientService.AcceptServiceProvider(serviceId, userId);
			DependencyResolver.Current.GetService<ServiceHubEntities>().SaveChanges();
			return PartialView("_BidAcceptanceServiceProviders", _clientService.GetBidsToAccept(serviceId));
		}


		[HttpPost]
		public ActionResult CancelServiceProvider(Guid userId, Guid serviceId, RatingClass ratingClass, string ratingComment)
		{
			_clientService.Rate(serviceId, userId, ratingClass, ratingComment);
			_clientService.CancelAcceptedBid(serviceId);
			DependencyResolver.Current.GetService<ServiceHubEntities>().SaveChanges();
			return PartialView("_BidAcceptanceServiceProviders", _clientService.GetBidsToAccept(serviceId));
		}


		[HttpPost]
		public ActionResult RateServiceProvider(Guid userId, Guid serviceId, RatingClass ratingClass, string ratingComment)
		{

			_clientService.Rate(serviceId,userId, ratingClass, ratingComment);
			DependencyResolver.Current.GetService<ServiceHubEntities>().SaveChanges();
			return PartialView("_BidAcceptanceServiceProviders", _clientService.GetBidsToAccept(serviceId));
		}


		[HttpPost]
		public ActionResult AddAdditionalInfo(Guid serviceId, string additionalInfo)
		{
			_clientService.AddAdditionalInfo(serviceId, additionalInfo);
			return Content("Additional info added!");
		}

		private void SetViewBagPostServiceData()
		{
			ViewBag.TagLookup = new SelectList(_lookupService.GetTags(), "Id", "Value");
			ViewBag.LocationLookup = new SelectList(_lookupService.GetLocations(), "Id", "Value");
		}



	}
}
