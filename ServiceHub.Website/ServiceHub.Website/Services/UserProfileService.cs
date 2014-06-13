using ServiceHub.Model;
using ServiceHub.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using System.Drawing;
using System.IO;
using System.Globalization;
using ServiceHub.Website.Controllers;
using System.Drawing.Imaging;

namespace ServiceHub.Website
{
	public sealed class UserProfileService
	{

		private readonly INotificationService _notificationService;
		private readonly ServiceHubEntities _serviceHubEntities;
		private readonly string _aspNetUserId;
		public UserProfileService(ServiceHubEntities serviceHubEntities, string aspNetUserId, INotificationService notificationService)
		{
			_serviceHubEntities = serviceHubEntities;
			_aspNetUserId = aspNetUserId;
			_notificationService = notificationService;
		}

		public void SaveUserProfile(Models.UserProfileViewModel userProfileViewModel)
		{
			if (userProfileViewModel == null)
				throw new ArgumentNullException("userProfileViewModel");

			User user = _serviceHubEntities.Users.SingleOrDefault(o => o.AspNetUserId == _aspNetUserId);

			if (user == null)
			{
				user = new User();
				_serviceHubEntities.Users.Add(user);
				user.AspNetUserId = _aspNetUserId;
			}

			user.About = userProfileViewModel.About ?? string.Empty;
			user.Name = userProfileViewModel.Name;
			user.ContactNumber = userProfileViewModel.ContactNumber;

			user.IsPublic = userProfileViewModel.IsPublic;

			foreach (Location location in user.Locations.ToList())
				user.Locations.Remove(location);
			foreach (Location location in _serviceHubEntities.Locations.Where(o => userProfileViewModel.Locations.Contains(o.Id)).ToList())
				user.Locations.Add(location);

			if (userProfileViewModel.Tags != null)
			{
				List<string> tagsText = userProfileViewModel.Tags.Split(',').ToList();

				List<Tag> existingTags = _serviceHubEntities.Tags.Where(o => tagsText.Contains(o.Title)).ToList();
				foreach (string tagText in tagsText)
				{
					if (!existingTags.Any(o => string.Equals(tagText, o.Title, StringComparison.OrdinalIgnoreCase)))
					{
						existingTags.Add(new Tag { Title = tagText });
					}
				}

				foreach (Tag tag in user.Tags.ToList())
					user.Tags.Remove(tag);
				foreach (Tag tag in existingTags.ToList())
					user.Tags.Add(tag);
			}

		}
		public Models.UserProfileViewModel GetUserProfile()
		{

			User user = _serviceHubEntities.Users.SingleOrDefault(o => o.AspNetUserId == _aspNetUserId);

			Models.UserProfileViewModel userProfileViewModel = new Models.UserProfileViewModel(user);

			return userProfileViewModel;

		}


		public byte[] GetLogoData()
		{
			UserProfileLogo logo = _serviceHubEntities.UserProfileLogos.SingleOrDefault(o => o.AspNetUserId == _aspNetUserId);

			if (logo == null)
				return null;

			return logo.LogoData;
		}

		
		public void SaveLogoData(byte[] logo)
		{
			UserProfileLogo userProfileLogo = _serviceHubEntities.UserProfileLogos.SingleOrDefault(o => o.AspNetUserId == _aspNetUserId);
			if (userProfileLogo == null)
			{
				userProfileLogo = new UserProfileLogo();
				_serviceHubEntities.UserProfileLogos.Add(userProfileLogo);
				userProfileLogo.AspNetUserId = _aspNetUserId;
			}

			ImageConverter imageConverter = new ImageConverter();
			using (Image img = (Image)imageConverter.ConvertFrom(logo))
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					ImageResizer imageResizer = new ImageResizer(300, 300, 70);
					imageResizer.ResizeImage(img, memoryStream);
					userProfileLogo.LogoData = memoryStream.ToArray();
				}
			}
		}


	}
}