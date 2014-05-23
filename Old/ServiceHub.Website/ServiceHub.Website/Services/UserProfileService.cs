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
using PagedList;
using WebMatrix.WebData;
using System.Globalization;
using ServiceHub.Website.Controllers;
using System.Drawing.Imaging;

namespace ServiceHub.Website
{
	public sealed class UserProfileService
	{
		private readonly ServiceHubEntities _serviceHubEntities;
		public UserProfileService(ServiceHubEntities serviceHubEntities)
		{
			_serviceHubEntities = serviceHubEntities;
		}

		public void SaveUserProfile(Models.UserProfileViewModel userProfileViewModel)
		{
			if (userProfileViewModel == null)
				throw new ArgumentNullException("userProfileViewModel");

			int userProfileId = WebSecurity.CurrentUserId;
			User user = _serviceHubEntities.Users.SingleOrDefault(o => o.UserProfileId == userProfileId);

			if (user == null)
			{
				user = new User();
				_serviceHubEntities.Users.Add(user);
				user.UserProfileId = userProfileId;
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
			int userProfileId = WebSecurity.CurrentUserId;

			User user = _serviceHubEntities.Users.SingleOrDefault(o => o.UserProfileId == userProfileId);

			Models.UserProfileViewModel userProfileViewModel = new Models.UserProfileViewModel(user);

			return userProfileViewModel;

		}


		public byte[] GetLogoData()
		{


			int userProfileId = WebSecurity.CurrentUserId;

			UserProfileLogo logo = _serviceHubEntities.UserProfileLogos.SingleOrDefault(o => o.UserProfileId == userProfileId);

			if (logo == null)
				return null;

			return logo.LogoData;
		}

		public byte[] GetLogoData(Guid userId)
		{

			User user = _serviceHubEntities.Users.SingleOrDefault(o => o.Id == userId);

			if (user == null)
				return null;

			byte[] image = user.UserProfile.UserProfileLogo.LogoData;
			return image;
		}

		public void SaveLogoData(byte[] logo)
		{
			int userProfileId = WebSecurity.CurrentUserId;

			UserProfileLogo userProfileLogo = _serviceHubEntities.UserProfileLogos.SingleOrDefault(o => o.UserProfileId == userProfileId);
			if (userProfileLogo == null)
			{
				userProfileLogo = new UserProfileLogo();
				_serviceHubEntities.UserProfileLogos.Add(userProfileLogo);
				userProfileLogo.UserProfileId = userProfileId;
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

		internal IEnumerable<HistoryItem> GetHistory()
		{

			int userProfileId = WebSecurity.CurrentUserId;
			DateTime aMonthAgo = DateTime.Now.AddMonths(-1);
			return
			_serviceHubEntities.AcceptedBids.Select(o => new { Timestamp = o.TimeStamp, Action = "Accepted Bid", Description = o.Bid.Service.Reference,UserProfileId =o.Bid.User.UserProfileId }).Concat(
			_serviceHubEntities.AdditionalInfoRequests.Select(o => new { Timestamp = o.TimeStamp, Action = "Requested Additional Info", Description = o.Service.Reference, UserProfileId = o.Service.User.UserProfileId })).Concat(
			_serviceHubEntities.AdditionalInfos.Select(o => new { Timestamp = o.TimeStamp, Action = "Added Additional Info", Description = o.Service.Reference, UserProfileId = o.Service.User.UserProfileId })).Concat(
			_serviceHubEntities.Bids.Select(o => new { Timestamp = o.TimeStamp, Action = "Bid on Service", Description = o.Service.Reference, UserProfileId = o.User.UserProfileId })).Concat(
			_serviceHubEntities.Services.Select(o => new { Timestamp = o.TimeStamp, Action = "Posted Service", Description = o.Reference, UserProfileId = o.User.UserProfileId }))
			.Where(o => o.UserProfileId==userProfileId && o.Timestamp >= aMonthAgo)
			.OrderByDescending(o => o.Timestamp).ToList()
			.Select(o => new HistoryItem(o.Timestamp, o.Action, o.Description));
		}


	}
}