using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceHub.Website.Models
{
	public sealed class ServiceBidViewModel
	{

		public ServiceBidViewModel()
		{
			AddtionalInfo = new List<string>();
			AddtionalInfoRequests = new List<string>();
		}

		public Guid ServiceId { get; set; }
		public Guid ServiceProviderId { get; set; }

		[Display(Name = "Highest Bid")]
		public decimal HighestBid { get; set; }

		[Display(Name = "My Highest Bid")]
		public decimal ServiceProviderCurrentBid { get; set; }

		[Required]
		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
		[Display(Name = "Bidding Completion Date")]
		public DateTime? BiddingCompletionDate { get; set; }

		[Required]
		[DisplayFormat(DataFormatString="{0:dd MMM yyyy}")]
		[Display(Name = "Service Date")]
		public DateTime? ServiceDate { get; set; }

		[Display(Name = "Reference")]
		public string Reference { get; set; }

		[Required]
		[Display(Name = "Description")]
		public string Description { get; set; }


		[Display(Name = "Location")]
		[Required]
		public string Location { get; set; }

		[Display(Name = "Service Tag")]
		[Required]
		public string ServiceTag { get; set; }


		[Display(Name = "Addtional Info")]
		[Required]
		public List<string> AddtionalInfo { get; set; }

		[Display(Name = "Addtional Info Requests")]
		[Required]
		public List<string> AddtionalInfoRequests { get; set; }

	}
}