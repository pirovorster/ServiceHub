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
using System.Data.SqlTypes;

namespace ServiceHub.Website
{
	public sealed class DirectoryService
	{
		private readonly ServiceHubEntities _serviceHubEntities;
		public DirectoryService(ServiceHubEntities serviceHubEntities)
		{
			_serviceHubEntities = serviceHubEntities;
		}

		public byte[] GetLogoData(Guid userId)
		{

			User user = _serviceHubEntities.Users.SingleOrDefault(o => o.Id == userId);

			if (user == null || user.AspNetUser.UserProfileLogo == null)
				return null;

			byte[] image = user.AspNetUser.UserProfileLogo.LogoData;
			return image;
		}

		public IPagedList<User> GetServiceProvidersPage(int page, int itemsPerPage, IEnumerable<int> locations, IEnumerable<Guid> tags, string searchString)
		{

			IQueryable<User> users = _serviceHubEntities.Users
					.Include("Tags")
					.Include("Ratings")
					.Include("Locations")
					.Where(o => o.IsPublic);

			if (locations.Count() > 0)
				locations = _serviceHubEntities.GetLocationsHierarchy(string.Join(",", locations)).Select(o => o.Id);

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

			if (locations.Count() > 0)
				locations = _serviceHubEntities.GetLocationsHierarchy(string.Join(",", locations)).Select(o => o.Id);

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



	}
}