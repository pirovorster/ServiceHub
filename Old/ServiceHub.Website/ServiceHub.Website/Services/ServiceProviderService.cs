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
	public sealed class ServiceProviderService
	{
		private readonly ServiceHubEntities _serviceHubEntities;
		public ServiceProviderService(ServiceHubEntities serviceHubEntities)
		{
			_serviceHubEntities = serviceHubEntities;
		}

		public void MakeBid(Guid serviceId, decimal bidValue)
		{
			int userProfileId = WebSecurity.CurrentUserId;
			User user = _serviceHubEntities.Users.SingleOrDefault(o => o.UserProfileId == userProfileId);

			Bid bid = new Bid
			{
				Amount = bidValue,
				IsCancelled = false,
				ServiceId = serviceId,
				TimeStamp = DateTime.Now

			};

			user.Bids.Add(bid);

		}

		public void RequestAdditionalInfo(Guid serviceId, Guid userId, string request)
		{
			AdditionalInfoRequest additionalInfoRequest = new AdditionalInfoRequest
			{
				Comment = request,
				UserId = userId,
				ServiceId = serviceId,
				TimeStamp = DateTime.Now,
			};

			_serviceHubEntities.AdditionalInfoRequests.Add(additionalInfoRequest);
		}

		internal IEnumerable<MyBidItem> MyBidItems()
		{
			int userProfileId = WebSecurity.CurrentUserId;

			DateTime aMonthAgo = DateTime.Now.AddMonths(-3);
			return
			_serviceHubEntities
			.Services
			.IncludeAll()
			.Where(o => o.Bids.Any(i => !i.IsCancelled && i.User.UserProfileId == userProfileId) && o.BiddingCompletionDate >= aMonthAgo)
			.ToList()
			.Select(o => new MyBidItem(o, o.UserId));
		}
		internal Models.ServiceBidViewModel GetServiceBid(Guid serviceId)
		{
			int userProfileId = WebSecurity.CurrentUserId;

			Service service = _serviceHubEntities.Services.IncludeAll()
				.Where(o => !o.IsCancelled)
				.SingleOrDefault(o => o.Id == serviceId);

			return new ServiceBidViewModel(service, _serviceHubEntities.Users.Single(o => o.UserProfileId == userProfileId));
		}

		public IPagedList<User> GetServiceProvidersPage(int page, int itemsPerPage, IEnumerable<int> locations, IEnumerable<Guid> tags, string searchString)
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

	}
}