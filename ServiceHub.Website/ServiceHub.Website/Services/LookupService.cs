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
		private readonly ServiceHubEntities _serviceHubEntities;
		public LookupService(ServiceHubEntities serviceHubEntities)
		{
			_serviceHubEntities = serviceHubEntities;
		}
		public IEnumerable<LookupValue> GetLocations()
		{

			return _serviceHubEntities.Locations.ToList().Select(o => new LookupValue(o.Id, o.Name)).ToList().AsReadOnly();

		}

		public IEnumerable<LookupValue> GetTags()
		{

			return _serviceHubEntities.Tags.ToList().Select(o => new LookupValue(o.Id, o.Title)).ToList().AsReadOnly();

		}
	}
}