using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceHub.Model
{
	public partial class ServiceProvider
	{
		public double Rating
		{
			get { return this.Ratings.Count ==0 ?0.5: this.Ratings.Average(o => o.Score); }
		}
	}
}
