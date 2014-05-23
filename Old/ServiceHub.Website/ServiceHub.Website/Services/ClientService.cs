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
	public sealed class ClientService
	{
		private readonly ServiceHubEntities _serviceHubEntities;

		public ClientService(ServiceHubEntities serviceHubEntities)
		{
			_serviceHubEntities = serviceHubEntities;
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
			return new DateTime(datetime.Year, datetime.Month, datetime.Day, 23, 59, 59);
		}


		

		internal Models.ServiceViewModel GetService(Guid serviceId)
		{
			int userProfileId = WebSecurity.CurrentUserId;

			Service service = _serviceHubEntities.Services.IncludeAll()
				.SingleOrDefault(o => o.Id == serviceId && o.User.UserProfileId == userProfileId);

			return new ServiceViewModel(service);
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


		internal IEnumerable<MyServiceItem> MyServiceItems()
		{
			int userProfileId = WebSecurity.CurrentUserId;

			DateTime aMonthAgo = DateTime.Now.AddMonths(-3);
			return
			_serviceHubEntities
			.Services
			.IncludeAll()
			.Where(o => (o.AcceptedBid == null && !o.IsCancelled) || (o.AcceptedBid != null && o.AcceptedBid.TimeStamp >= aMonthAgo) || o.BiddingCompletionDate >= aMonthAgo)
			.ToList()
			.Select(o => new MyServiceItem(o));
		}


		internal void CancelService(Guid serviceId)
		{
			int userProfileId = WebSecurity.CurrentUserId;

			Service service = _serviceHubEntities.Services
				.SingleOrDefault(o => o.Id == serviceId && o.User.UserProfileId == userProfileId);


			if (service != null)
			{
				service.IsCancelled = true;
			}

		}

		internal void AddAdditionalInfo(Guid serviceId, string additionalInfo)
		{
			int userProfileId = WebSecurity.CurrentUserId;

			Service service = _serviceHubEntities.Services
				.Include("AdditionalInfos")
				.SingleOrDefault(o => o.Id == serviceId && o.User.UserProfileId == userProfileId);


			if (service != null)
			{
				if (!service.AdditionalInfos.Select(o => o.Comment).Contains(additionalInfo))
					service.AdditionalInfos.Add(new AdditionalInfo { Comment = additionalInfo, TimeStamp = DateTime.Now });
			}
		}

		internal BidAcceptanceViewModel GetBidsToAccept(Guid serviceId)
		{
			int userProfileId = WebSecurity.CurrentUserId;

			Service service = _serviceHubEntities.Services
				.Include("AdditionalInfos")
				.SingleOrDefault(o => o.Id == serviceId && o.User.UserProfileId == userProfileId);

			return new BidAcceptanceViewModel(service);
		}
	}
}