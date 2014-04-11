using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ServiceHub.Model
{
	public static class Extensions
	{
		public static IEnumerable<Bid> LatestBids(this Service service)
		{
			return service
				.Bids
				.Where(o => !o.IsCancelled)
				.GroupBy(o => o.UserId)
				.Select(o => o.OrderBy(i => i.TimeStamp).FirstOrDefault())
				.Where(o => o != null);
		}

		public static Bid LatestBidForUser(this Service service, Guid userId)
		{
			return service.Bids
				.Where(o => !o.IsCancelled && o.UserId == userId)
				.OrderBy(o => o.TimeStamp)
				.FirstOrDefault();
		}

		public static IQueryable< Service> IncludeAll(this DbQuery< Service> services)
		{
			return services.Include(o => o.Tag)
				.Include(o => o.Location)
				.Include(o => o.Bids)
				.Include(o => o.AdditionalInfoRequests)
				.Include(o => o.AdditionalInfos)
				.Include(o => o.User);
		}

		
	}
}
