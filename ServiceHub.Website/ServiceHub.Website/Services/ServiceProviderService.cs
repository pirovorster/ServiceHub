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

namespace ServiceHub.Website
{
	public sealed class ServiceProviderService
	{

		public IPagedList<ServiceProvider> GetServiceProviderPage(int page, int itemsPerPage, IEnumerable<int> locations, IEnumerable<Guid> tags, string searchString)
		{

			using (ServiceHubEntities serviceHubEntities = new ServiceHubEntities())
			{
				IQueryable<ServiceProvider> serviceProviders = serviceHubEntities.ServiceProviders
					.Include("Client")
					.Include("Tags")
					.Include("Ratings")
					.Include("Locations")
					.Where(o => o.IsActive);

				if (locations.Count() > 0 && tags.Count() > 0)
					serviceProviders = serviceProviders.Where(o => o.Locations.Any(i => locations.Contains(i.Id)) || o.Tags.Any(i => tags.Contains(i.Id)));
				else if (locations.Count() > 0)
					serviceProviders = serviceProviders.Where(o => o.Locations.Any(i => locations.Contains(i.Id)));
				else if (tags.Count() > 0)
					serviceProviders = serviceProviders.Where(o => o.Tags.Any(i => tags.Contains(i.Id)));

				if (!string.IsNullOrWhiteSpace(searchString))
				{
					string searchStringUpperCase = searchString.ToUpper();
					serviceProviders = serviceProviders.Where(o => o.Client.Name.ToUpper().Contains(searchStringUpperCase));
				}

				return serviceProviders
					.OrderBy(o => o.Ratings.Average(i => i.Score))
					.ToPagedList(page, itemsPerPage);
			}
		}

		public byte[] GetLogoData(Guid serviceProviderId)
		{

			using (ServiceHubEntities serviceHubEntities = new ServiceHubEntities())
			{
				return serviceHubEntities.ServiceProviders.Single(o => o.ClientId == serviceProviderId).Logo;

			}
		}
	}
}