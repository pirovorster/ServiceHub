using Foolproof;
using ServiceHub.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceHub.Website.Models
{
	public sealed class ServiceBidViewModel
	{

		public Guid _serviceId;
		public Guid _userId;
		public decimal _lowestBid;
		public decimal _userCurrentBid;
		public DateTime? _biddingCompletionDate;
		public DateTime? _serviceDate;
		public string _reference;
		public string _description;
		public string _location;
		public string _serviceTag;
		public List<string> _addtionalInfo;
		public List<string> _addtionalInfoRequests;

		public ServiceBidViewModel(Model.Service service, User user)
		{
			decimal lowestBid =
				
				service
				.LatestBids()
				.Select(o => o.Amount)
				.OrderBy(o=>o)
				.FirstOrDefault();
				

			Bid latestBid = service
				.LatestBidForUser(user.Id);

			decimal userCurrentBid = latestBid == null ? 0 : latestBid.Amount;
				
			if (service != null)
			{
				_userId = user.Id;
				_serviceId = service.Id;
				_lowestBid =  lowestBid;
				_userCurrentBid = userCurrentBid;
				_biddingCompletionDate = service.BiddingCompletionDate;
				_description = service.Description;
				_location = service.Location.Name;
				_reference = service.Reference;
				_serviceDate = service.ServiceDue;
				_serviceTag = service.Tag.Title;
				_addtionalInfo = service.AdditionalInfos.Select(o => o.Comment).ToList();
				_addtionalInfoRequests = service.AdditionalInfoRequests.Select(o => o.Comment).ToList();

			}
			else
			{
				_addtionalInfo = new List<string>();
				_addtionalInfoRequests = new List<string>();
			}
		}

		public Guid ServiceId { get { return _serviceId; } }
		public Guid UserId { get { return _userId; } }

		[Display(Name = "Lowest Bid")]
		public decimal LowestBid { get { return _lowestBid; } }

		[Display(Name = "My Lowest Bid")]
		public decimal UserCurrentBid { get { return _userCurrentBid; } }

		[Required]
		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
		[Display(Name = "Bidding Completion Date")]
		public DateTime? BiddingCompletionDate { get { return _biddingCompletionDate; } }

		[Required]
		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
		[Display(Name = "Service Date")]
		public DateTime? ServiceDate { get { return _serviceDate; } }

		[Display(Name = "Reference")]
		public string Reference { get { return _reference; } }

		[Required]
		[Display(Name = "Description")]
		public string Description { get { return _description; } }


		[Display(Name = "Location")]
		[Required]
		public string Location { get { return _location; } }

		[Display(Name = "Service Tag")]
		[Required]
		public string ServiceTag { get { return _serviceTag; } }


		[Display(Name = "Addtional Info")]
		[Required]
		public List<string> AddtionalInfo { get { return _addtionalInfo; } }

		[Display(Name = "Addtional Info Requests")]
		[Required]
		public List<string> AddtionalInfoRequests { get { return _addtionalInfoRequests; } }

	}
}