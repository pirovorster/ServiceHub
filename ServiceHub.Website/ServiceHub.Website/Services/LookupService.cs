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

namespace ServiceHub.Website
{
	public sealed class LookupService
	{	
		public IEnumerable<LookupValue> GetLocations()
		{
			using (ServiceHubEntities serviceHubEntities = new ServiceHubEntities())
			{
				return serviceHubEntities.Locations.ToList().Select(o => new LookupValue(o.Id, o.Name)).ToList().AsReadOnly();
			}
		}

		public IEnumerable<LookupValue> GetTags()
		{
			using (ServiceHubEntities serviceHubEntities = new ServiceHubEntities())
			{
				return serviceHubEntities.Tags.ToList().Select(o => new LookupValue(o.Id, o.Title)).ToList().AsReadOnly();


			}
		}
	}
}