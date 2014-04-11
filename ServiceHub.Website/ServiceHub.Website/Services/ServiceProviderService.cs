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
using System.Globalization;
using ServiceHub.Website.Controllers;
using System.Drawing.Imaging;

namespace ServiceHub.Website
{
	public sealed class ServiceProviderService
	{
		private readonly ServiceHubEntities _serviceHubEntities;
		private readonly string _aspNetUserId;
		
		public ServiceProviderService(ServiceHubEntities serviceHubEntities,string aspNetUserId)
		{
			_serviceHubEntities = serviceHubEntities;
			_aspNetUserId = aspNetUserId;
		}

		public void MakeBid(Guid serviceId, decimal bidValue)
		{
			User user = _serviceHubEntities.Users.SingleOrDefault(o => o.AspNetUserId == _aspNetUserId);

			Bid bid = new Bid
			{
				Amount = bidValue,
				IsCancelled = false,
				ServiceId = serviceId,
				TimeStamp = DateTime.Now

			};

			user.Bids.Add(bid);

		}

		public void RequestAdditionalInfo(Guid serviceId, string request)
		{
			User user = _serviceHubEntities.Users.SingleOrDefault(o => o.AspNetUserId == _aspNetUserId);


			AdditionalInfoRequest additionalInfoRequest = new AdditionalInfoRequest
			{
				Comment = request,
				UserId = user.Id,
				ServiceId = serviceId,
				TimeStamp = DateTime.Now,
			};

			_serviceHubEntities.AdditionalInfoRequests.Add(additionalInfoRequest);
		}

		internal IEnumerable<MyBidItem> MyBidItems()
		{
			
			DateTime aMonthAgo = DateTime.Now.AddMonths(-3);
			return
			_serviceHubEntities
			.Services
			.IncludeAll()
			.Where(o => o.Bids.Any(i => !i.IsCancelled && i.User.AspNetUserId == _aspNetUserId) && o.BiddingCompletionDate >= aMonthAgo)
			.ToList()
			.Select(o => new MyBidItem(o, o.UserId));
		}
		internal Models.ServiceBidViewModel GetServiceBid(Guid serviceId)
		{
			
			Service service = _serviceHubEntities.Services.IncludeAll()
				.Where(o => !o.IsCancelled)
				.SingleOrDefault(o => o.Id == serviceId);

			return new ServiceBidViewModel(service, _serviceHubEntities.Users.Single(o => o.AspNetUserId == _aspNetUserId));
		}

		

	}
}