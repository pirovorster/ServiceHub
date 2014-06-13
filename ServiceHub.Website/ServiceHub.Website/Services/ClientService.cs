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
using System.Data.SqlTypes;
using ServiceHub.Website.Models;
using ServiceHub.Website.Services;


namespace ServiceHub.Website
{
	public sealed class ClientService
	{

		private readonly INotificationService _notificationService;
		private readonly ServiceHubEntities _serviceHubEntities;
		private readonly string _aspNetUserId;
		public ClientService(ServiceHubEntities serviceHubEntities, string aspNetUserId, INotificationService notificationService)
		{
			_serviceHubEntities = serviceHubEntities;
			_aspNetUserId = aspNetUserId;
			_notificationService = notificationService;
		}
		internal Models.ServiceViewModel GetService(Guid serviceId)
		{
			Service service = _serviceHubEntities.Services.IncludeAll()
				.SingleOrDefault(o => o.Id == serviceId && o.User.AspNetUserId == _aspNetUserId);

			return new ServiceViewModel(service);
		}

		public void PostService(Models.PostServiceViewModel postServiceViewModel)
		{

			User user = _serviceHubEntities.Users.SingleOrDefault(o => o.AspNetUserId == _aspNetUserId);
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

		private static DateTime GetEndOfDay(DateTime datetime)
		{
			return new DateTime(datetime.Year, datetime.Month, datetime.Day, 23, 59, 59);
		}

		internal IEnumerable<MyServiceItem> MyServiceItems()
		{
			return
			_serviceHubEntities
			.Services
			.IncludeAll()
			.Where(o => o.User.AspNetUserId == _aspNetUserId)
			.OrderByDescending(o => o.TimeStamp)
			.ToList()
			.Select(o => new MyServiceItem(o));
		}


		internal void CancelService(Guid serviceId)
		{

			Service service = _serviceHubEntities.Services
				.SingleOrDefault(o => o.Id == serviceId && o.User.AspNetUserId == _aspNetUserId);


			if (service != null)
			{
				service.IsCancelled = true;
			}

		}

		internal void AddAdditionalInfo(Guid serviceId, string additionalInfo)
		{
			Service service = _serviceHubEntities.Services
				.Include("AdditionalInfos")
				.SingleOrDefault(o => o.Id == serviceId && o.User.AspNetUserId == _aspNetUserId);

			if (service != null)
			{
				if (!service.AdditionalInfos.Select(o => o.Comment).Contains(additionalInfo))
					service.AdditionalInfos.Add(new AdditionalInfo { Comment = additionalInfo, TimeStamp = DateTime.Now });
			}
		}

		internal BidAcceptanceViewModel GetBidsToAccept(Guid serviceId)
		{
			Service service = _serviceHubEntities.Services
				.Include("AdditionalInfos")
				.SingleOrDefault(o => o.Id == serviceId && o.User.AspNetUserId == _aspNetUserId);

			return new BidAcceptanceViewModel(service);
		}

		internal void AcceptServiceProvider(Guid serviceId, Guid userId)
		{
			Service service = _serviceHubEntities.Services
			.SingleOrDefault(o => o.Id == serviceId && o.User.AspNetUserId == _aspNetUserId);
			Bid bid = _serviceHubEntities.Bids
				.Where(o => o.ServiceId == serviceId && o.UserId == userId).OrderByDescending(o => o.TimeStamp).FirstOrDefault();

			if (service != null && bid != null)
			{
				if (bid.AcceptedBid == null)
					service.AcceptedBid = new AcceptedBid { Bid = bid, TimeStamp = DateTime.Now };
				else
				{
					service.AcceptedBid = bid.AcceptedBid;
					service.AcceptedBid.TimeStamp = DateTime.Now;
					service.AcceptedBid.IsCancelled = false;
				}

			}

		}

		internal void CancelAcceptedBid(Guid serviceId)
		{
			Service service = _serviceHubEntities.Services
			.SingleOrDefault(o => o.Id == serviceId && o.User.AspNetUserId == _aspNetUserId);

			if (service != null)
			{
				service.AcceptedBid.IsCancelled = true;
				service.AcceptedBid = null;
			}

		}

		internal void Rate(Guid serviceId, Guid userId, RatingClass ratingClass, string ratingComment)
		{
			Service service = _serviceHubEntities.Services
			.SingleOrDefault(o => o.Id == serviceId && o.User.AspNetUserId == _aspNetUserId);
			Bid bid = _serviceHubEntities.Bids
				.Where(o => o.ServiceId == serviceId && o.UserId == userId).OrderByDescending(o => o.TimeStamp).FirstOrDefault();

			if (service != null && bid != null)
			{
				if (service.AcceptedBid.Rating == null)
					service.AcceptedBid.Rating = new Rating { UserId = userId, Comment = ratingComment, Score = (int)ratingClass / 100.0, TimeStamp = DateTime.Now };
				else
				{
					service.AcceptedBid.Rating.UserId = userId;

					service.AcceptedBid.Rating.Comment = ratingComment;

					service.AcceptedBid.Rating.Score = (int)ratingClass / 100.0;

					service.AcceptedBid.Rating.TimeStamp = DateTime.Now;
				}
			}
		}
	}
}