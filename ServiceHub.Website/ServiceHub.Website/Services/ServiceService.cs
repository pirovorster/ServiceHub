using ServiceHub.Model;
using Models = ServiceHub.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using System.Drawing;
using System.IO;
using System.Globalization;
using PagedList;
using WebMatrix.WebData;
using System.Data.SqlTypes;
using ServiceHub.Website.Models;

namespace ServiceHub.Website
{
	public sealed class ServiceService
	{
		private readonly ServiceHubEntities _serviceHubEntities;

		public ServiceService(ServiceHubEntities serviceHubEntities)
		{
			_serviceHubEntities = serviceHubEntities;
		}


		public void Bid(Guid serviceId, decimal bidValue)
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
			_serviceHubEntities.SaveChanges();

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
			_serviceHubEntities.SaveChanges();

		}

		internal IPagedList<Service> GetServicesPage(int page, 
			int itemsPerPage, 
			IEnumerable<int> locations, 
			IEnumerable<Guid> tags, 
			string searchString,
			DateTime? beginBiddingCompletionDate,
			DateTime? endBiddingCompletionDate,
			DateTime? beginEstimatedServiceDate,
			DateTime? endEstimatedServiceDate)
		{

			DateTime beginBiddingCompletionDateValue = beginBiddingCompletionDate.HasValue ? beginBiddingCompletionDate.Value : SqlDateTime.MinValue.Value;
			DateTime endBiddingCompletionDateValue = endBiddingCompletionDate.HasValue ? endBiddingCompletionDate.Value : SqlDateTime.MaxValue.Value;
			DateTime beginEstimatedServiceDateValue = beginEstimatedServiceDate.HasValue ? beginEstimatedServiceDate.Value : SqlDateTime.MinValue.Value;
			DateTime endEstimatedServiceDateValue = endEstimatedServiceDate.HasValue ? endEstimatedServiceDate.Value : SqlDateTime.MaxValue.Value;

			beginBiddingCompletionDateValue = GetEndOfDay(beginBiddingCompletionDateValue);
			endBiddingCompletionDateValue = GetEndOfDay(endBiddingCompletionDateValue);
			beginEstimatedServiceDateValue = GetEndOfDay(beginEstimatedServiceDateValue);
			endEstimatedServiceDateValue = GetEndOfDay(endEstimatedServiceDateValue);


			IQueryable<Service> services = _serviceHubEntities.Services
				.Include("Tag")
				.Include("Location")
				.Where(o => !o.IsCancelled);

			if (locations.Count() > 0 && tags.Count() > 0)
				services = services.Where(o => locations.Contains(o.LocationId) || tags.Contains(o.TagId));
			else if (locations.Count() > 0)
				services = services.Where(o => locations.Contains(o.LocationId));
			else if (tags.Count() > 0)
				services = services.Where(o => tags.Contains(o.TagId));

			services.Where(o => beginBiddingCompletionDateValue <= o.BiddingCompletionDate && o.BiddingCompletionDate <= endBiddingCompletionDateValue);

			services.Where(o => beginEstimatedServiceDateValue <= o.ServiceDue && o.ServiceDue <= endEstimatedServiceDateValue);

			if (!string.IsNullOrWhiteSpace(searchString))
			{
				string searchStringUpperCase = searchString.ToUpper();
				services = services.Where(o => o.Description.ToUpper().Contains(searchStringUpperCase));
			}
			

			return services
				.OrderByDescending(o => o.TimeStamp)
				.ToPagedList(page, itemsPerPage);

		}
		private static DateTime GetEndOfDay(DateTime datetime)
		{
			return new DateTime(datetime.Year, datetime.Month, datetime.Day, 23,59, 59);
		}


		internal Models.ServiceBidViewModel GetService(Guid serviceId)
		{
			int userProfileId = WebSecurity.CurrentUserId;

			Service service = _serviceHubEntities.Services
				.Include("Tag")
				.Include("Location")
				.Where(o => !o.IsCancelled)
				.SingleOrDefault(o => o.Id == serviceId);

			decimal highestBid =
				new List<decimal>
					{
						0
					}.Concat(
				service
				.Bids
				.Where(o => !o.IsCancelled)
				.GroupBy(o => o.UserId)
				.Select(o => o.OrderByDescending(i => i.TimeStamp).FirstOrDefault())
				.Where(o => o != null)
				.Select(o => o.Amount))
				.Max(o => o);

			decimal userCurrentBid = new List<decimal>
					{
						0
					}.Concat(service
				.Bids
				.Where(o => o.User.UserProfileId == userProfileId && !o.IsCancelled)
				.OrderByDescending(i => i.TimeStamp)
				.Select(o => o.Amount))
				.Max(o => o);

			if (service != null)
				return new Models.ServiceBidViewModel
				{
					UserId = _serviceHubEntities.Users.Single(o => o.UserProfileId == userProfileId).Id,
					ServiceId = serviceId,
					HighestBid = highestBid,
					UserCurrentBid = userCurrentBid,
					BiddingCompletionDate = service.BiddingCompletionDate,
					Description = service.Description,
					Location = service.Location.Name,
					Reference = service.Reference,
					ServiceDate = service.ServiceDue,
					ServiceTag = service.Tag.Title,
					AddtionalInfo = service.AdditionalInfos.Select(o => o.Comment).ToList(),
					AddtionalInfoRequests = service.AdditionalInfoRequests.Select(o => o.Comment).ToList(),

				};
			else
				return new Models.ServiceBidViewModel();


		}

		public void PostService(Models.PostServiceViewModel postServiceViewModel)
		{
			int userProfileId = WebSecurity.CurrentUserId;

			User user = _serviceHubEntities.Users.SingleOrDefault(o => o.UserProfileId == userProfileId);
			Tag tag = _serviceHubEntities.Tags.SingleOrDefault(o => o.Id == postServiceViewModel.ServiceTagId);
			Location location = _serviceHubEntities.Locations.SingleOrDefault(o => o.Id == postServiceViewModel.LocationId);

			Service service = new Service();
			service.Description = postServiceViewModel.Description;
			service.User = user;
			service.BiddingCompletionDate = GetEndOfDay(postServiceViewModel.BiddingCompletionDate.Value);
			service.ServiceDue = GetEndOfDay(postServiceViewModel.ServiceDate.Value);
			service.Tag = tag;
			service.Location = location;
			service.TimeStamp = DateTime.Now;
			service.IsCancelled = false;
			postServiceViewModel.Reference = service.Reference = string.Format(CultureInfo.InvariantCulture, "S{0}", DateTime.Now.Ticks);

			_serviceHubEntities.Services.Add(service);
		}

		internal IEnumerable<MyBidItem> MyBidItems()
		{
			int userProfileId = WebSecurity.CurrentUserId;

			DateTime aMonthAgo = DateTime.Now.AddMonths(-3);
			return
			_serviceHubEntities
			.Services.Where(o => o.Bids.Any(i=>!i.IsCancelled && i.User.UserProfileId == userProfileId) && o.BiddingCompletionDate >= aMonthAgo)
			.ToList()
			.Select(o => new MyBidItem(o, o.UserId));
		}
		
	}
}