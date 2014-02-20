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

		public IPagedList<ServiceProvider> GetServiceProviderPage(int page, int itemsPerPage)
		{

			using (ServiceHubEntities serviceHubEntities = new ServiceHubEntities())
			{
				return serviceHubEntities.ServiceProviders.Include("Client").Include("Tags").Include("Ratings").Where(o => o.IsActive).OrderBy(o => o.Ratings.Average(i => i.Score)).ToPagedList(page, itemsPerPage);

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