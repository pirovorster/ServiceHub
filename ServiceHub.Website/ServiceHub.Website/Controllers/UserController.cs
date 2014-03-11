﻿using System;
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
using System.Globalization;
using System.IO;

namespace ServiceHub.Website.Controllers
{
	[Authorize]

	public class UserController : BaseController
	{
		LookupService _lookupService;
		ServiceService _serviceService;
		UserService _userService;
		public UserController(LookupService lookupService, ServiceService serviceService, UserService userService)
		{
			_lookupService = lookupService;
			_serviceService = serviceService;
			_userService = userService;

		}
		public ActionResult UserProfile()
		{

			JavaScriptSerializer serializer = new JavaScriptSerializer();

			UserProfileViewModel userProfileViewModel = _userService.GetUserProfile();

			ViewBag.Message = string.Empty;
			SetViewBagData();
			return View(userProfileViewModel);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult UserProfile(UserProfileViewModel userProfileViewModel)
		{

			if (ModelState.IsValid)
			{
				_userService.SaveUser(userProfileViewModel);

				ViewBag.Message = "User Profile has been saved!";
			}

			SetViewBagData();
			return View(userProfileViewModel);
		}

		public ActionResult MyHistory()
		{
			return View(_userService.GetHistory());
		}

		public void UploadImage(HttpPostedFileBase imageFile)
		{
			if (imageFile != null)
			{
				byte[] logo = new byte[imageFile.ContentLength];
				imageFile.InputStream.Read(logo, 0, imageFile.ContentLength);
				_userService.SaveLogoData(logo);
			}
		}

		public ActionResult GetProfilePicture()
		{
			byte[] image = _userService.GetLogoData();

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
