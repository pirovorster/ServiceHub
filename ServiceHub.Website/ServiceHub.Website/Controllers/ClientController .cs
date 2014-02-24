using ServiceHub.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace ServiceHub.Website.Controllers
{
	public class ClientController : Controller
	{
		private readonly LookupService _lookupService;
		private readonly ClientService _clientService;
		public ClientController()
		{
			int userId = WebSecurity.CurrentUserId;
			_lookupService = new LookupService();
			_clientService = new ClientService(userId);
		}
		public ActionResult Services()
		{

			return View();
		}

		public ActionResult MyServices()
		{
			return View(Guid.NewGuid());
		}

		[HttpGet]
		public ActionResult PostService()
		{
			SetViewBagData();
			return View();
		}

		[HttpPost]
		public ActionResult PostService(PostServiceViewModel postServiceViewModel)
		{
			if (ModelState.IsValid)
			{
				_clientService.PostService(postServiceViewModel);

				SetViewBagData();
				return View(postServiceViewModel);
			}

			SetViewBagData();
			return View(postServiceViewModel);
		}


		private void SetViewBagData()
		{
			ViewBag.TagLookup = new SelectList(_lookupService.GetTags(), "Id", "Value");
			ViewBag.LocationLookup = new SelectList(_lookupService.GetLocations(), "Id", "Value");
		}


	}
}
