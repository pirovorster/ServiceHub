using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using ServiceHub.Website.Filters;
using ServiceHub.Website.Models;
using System.Web.Script.Serialization;

namespace ServiceHub.Website.Controllers
{
	[Authorize]

	public class UserController : Controller
	{
		LookupService _lookupService;
		ClientService _clientService;
		public UserController()
		{
			_lookupService = new LookupService();
			_clientService=new ClientService();
		}

		public ActionResult UserProfile()
		{
		
			JavaScriptSerializer serializer = new JavaScriptSerializer();

			UserProfileViewModel userProfileViewModel = _clientService.GetUserProfile();

			ViewBag.Message = string.Empty;
			SetViewBagData();
			return View(userProfileViewModel);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult UserProfile(UserProfileViewModel userProfileViewModel, HttpPostedFileBase logo)
		{

			if (ModelState.IsValid)
			{
				if (logo != null)
				{
					//product.ImageMimeType = image.ContentType;
					userProfileViewModel.LogoData = new byte[logo.ContentLength];
					logo.InputStream.Read(userProfileViewModel.LogoData, 0, logo.ContentLength);
				}
				_clientService.SaveUserProfile(userProfileViewModel);

				ViewBag.Message = "User Profile has been saved!";
			}

			SetViewBagData();
			return View(userProfileViewModel);
		}

		private void SetViewBagData()
		{
			ViewBag.TagLookup = _lookupService.GetTags().Select(o => o.Value);
			ViewBag.LocationLookup = new MultiSelectList(_lookupService.GetLocations(), "Id", "Value");
		}

		
	}
}
