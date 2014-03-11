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

namespace ServiceHub.Website
{
	public sealed class UserService
	{
		private readonly ServiceHubEntities _serviceHubEntities;
		public UserService(ServiceHubEntities serviceHubEntities)
		{
			_serviceHubEntities = serviceHubEntities;
		}

		public void SaveUser(Models.UserProfileViewModel userProfileViewModel)
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
		public IPagedList<User> GetUsersPage(int page, int itemsPerPage, IEnumerable<int> locations, IEnumerable<Guid> tags, string searchString)
		{

			IQueryable<User> users = _serviceHubEntities.Users
					.Include("Tags")
					.Include("Ratings")
					.Include("Locations")
					.Where(o => o.IsPublic);

			if (locations.Count() > 0 && tags.Count() > 0)
				users = users.Where(o => o.Locations.Any(i => locations.Contains(i.Id)) || o.Tags.Any(i => tags.Contains(i.Id)));
			else if (locations.Count() > 0)
				users = users.Where(o => o.Locations.Any(i => locations.Contains(i.Id)));
			else if (tags.Count() > 0)
				users = users.Where(o => o.Tags.Any(i => tags.Contains(i.Id)));

			if (!string.IsNullOrWhiteSpace(searchString))
			{
				string searchStringUpperCase = searchString.ToUpper();
				users = users.Where(o => o.Name.ToUpper().Contains(searchStringUpperCase));
			}

			return users
				.OrderByDescending(o => o.Ratings.Average(i => i.Score))
				.ToPagedList(page, itemsPerPage);
		}

		public byte[] GetLogoData()
		{
			int userProfileId = WebSecurity.CurrentUserId;

			byte[] image = _serviceHubEntities.Users.Single(o => o.UserProfileId == userProfileId).Logo;

			

			return image;
		}

		public byte[] GetLogoData(Guid userId)
		{
			byte[] image = _serviceHubEntities.Users.Single(o => o.Id == userId).Logo;
			return image;
		}

		public void  SaveLogoData(byte[] logo)
		{
			int userProfileId = WebSecurity.CurrentUserId;

			User user = _serviceHubEntities.Users.SingleOrDefault(o => o.UserProfileId == userProfileId);

			if (user == null)
			{
				user = new User();
				_serviceHubEntities.Users.Add(user);
				user.UserProfileId = userProfileId;
			}

			user.Logo = logo;

			_serviceHubEntities.SaveChanges();
		
		}


		public Models.UserProfileViewModel GetUserProfile()
		{
			Models.UserProfileViewModel userProfileViewModel = new Models.UserProfileViewModel();

			int userProfileId = WebSecurity.CurrentUserId;

			User user = _serviceHubEntities.Users.SingleOrDefault(o => o.UserProfileId == userProfileId);
			if (user == null)
				return userProfileViewModel;

			userProfileViewModel.Name = user.Name;
			userProfileViewModel.ContactNumber = user.ContactNumber;
			userProfileViewModel.About = user.About;
			userProfileViewModel.LogoData = user.Logo;

			userProfileViewModel.Tags = string.Join(",", user.Tags.Select(o => o.Title));
			userProfileViewModel.Locations = user.Locations.Select(o => o.Id).ToList();
		
			return userProfileViewModel;

		}



		internal IEnumerable<HistoryItem> GetHistory()
		{

			DateTime aMonthAgo=DateTime.Now.AddMonths(-1);
			return
			_serviceHubEntities.AcceptedBids.Select(o=>new {Timestamp = o.TimeStamp, Action ="Accepted Bid", Description=o.Bid.Service.Reference}).Concat(
			_serviceHubEntities.AdditionalInfoRequests.Select(o=>new {Timestamp = o.TimeStamp, Action ="Requested Additional Info", Description=o.Service.Reference})).Concat(
			_serviceHubEntities.AdditionalInfos.Select(o=>new {Timestamp = o.TimeStamp, Action ="Added Additional Info", Description=o.Service.Reference})).Concat(
			_serviceHubEntities.Bids.Select(o=>new {Timestamp = o.TimeStamp, Action ="Bid on Service", Description=o.Service.Reference})).Concat(
			_serviceHubEntities.Services.Select(o=>new {Timestamp = o.TimeStamp, Action ="Posted Service", Description=o.Reference}))
			.Where(o=>o.Timestamp>=aMonthAgo)
			.OrderByDescending(o=>o.Timestamp).ToList()
			.Select(o=>new HistoryItem(o.Timestamp,o.Action,o.Description));
		}

		
	}
}