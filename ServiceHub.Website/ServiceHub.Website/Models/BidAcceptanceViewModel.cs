using ServiceHub.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceHub.Website.Models
{
	public sealed class BidAcceptanceViewModel
	{
		private readonly ServiceViewModel _serviceViewModel;


		public BidAcceptanceViewModel(Service service)
		{
			_serviceViewModel = new ServiceViewModel(service);

			if (service != null)
			{

				FinalServiceProviderBidViewModels = service
				.LatestBids().Select(o => new FinalServiceProviderBidViewModel(o)).ToList();
			}
			else
				FinalServiceProviderBidViewModels = new List<FinalServiceProviderBidViewModel>();
		}
		public ServiceViewModel ServiceViewModel { get { return _serviceViewModel; } }

		public List<FinalServiceProviderBidViewModel> FinalServiceProviderBidViewModels { get; set; }
	}
}