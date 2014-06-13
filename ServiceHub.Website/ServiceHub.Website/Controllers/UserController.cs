using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ServiceHub.Website.Models;
using System.Web.Script.Serialization;
using System.Globalization;
using System.IO;

namespace ServiceHub.Website.Controllers
{
	[Authorize]
	public class UserController : BaseBusinessController
	{
		LookupService _lookupService;
		UserProfileService _userProfileService;
		public UserController(LookupService lookupService, UserProfileService userProfileService)
		{
			_lookupService = lookupService;
			_userProfileService = userProfileService;

		}


		[HttpGet]
		public ActionResult UserProfile()
		{

			JavaScriptSerializer serializer = new JavaScriptSerializer();

			UserProfileViewModel userProfileViewModel = _userProfileService.GetUserProfile();

			ViewBag.Message = string.Empty;
			SetViewBagData();
			return View(userProfileViewModel);
		}

		[HttpPost]
		public ActionResult UserProfile(UserProfileViewModel userProfileViewModel)
		{

			if (ModelState.IsValid)
			{
				_userProfileService.SaveUserProfile(userProfileViewModel);

				ViewBag.Message = "User Profile has been saved!";
			}

			SetViewBagData();
			return View(userProfileViewModel);
		}


		[HttpPost]
		public void UploadImage(HttpPostedFileBase imageFile)
		{
			if (imageFile != null)
			{
				byte[] logo = new byte[imageFile.ContentLength];
				imageFile.InputStream.Read(logo, 0, imageFile.ContentLength);
				_userProfileService.SaveLogoData(logo);
			}
		}


		[HttpGet]
		public ActionResult GetProfilePicture()
		{
			byte[] image = _userProfileService.GetLogoData();

			if (image == null)
			{
				return new FilePathResult("~/Images/noimage.png", "image/png");
			}
			else
			{
				return new FileContentResult(image, "image/jpeg");
			}
		}



		private void SetViewBagData()
		{
			ViewBag.TagLookup = _lookupService.GetTags().Select(o => o.Value);
			ViewBag.LocationLookup = new MultiSelectList(_lookupService.GetLocations(), "Id", "Value");
		}


	}
}
