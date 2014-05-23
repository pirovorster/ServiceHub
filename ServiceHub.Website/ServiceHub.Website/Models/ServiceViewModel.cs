using Foolproof;
using ServiceHub.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceHub.Website.Models
{
	public sealed class ServiceViewModel
	{

		private readonly Guid _serviceId;
		private readonly DateTime _biddingCompletionDate;
		private readonly DateTime _serviceDate;
		private readonly string _reference;
		private readonly string _description;
		private readonly string _location;
		private readonly string _serviceTag;
		private readonly List<string> _addtionalInfo;
		private readonly List<string> _addtionalInfoRequests;
		private readonly bool _isCancelled;
		private readonly bool _isAccepted;
		public ServiceViewModel(Service service)
		{
		
			if (service != null)
			{
				_serviceId = service.Id;

				_biddingCompletionDate = service.BiddingCompletionDate;
				_description = service.Description;
				_location = service.Location.Name;
				_reference = service.Reference;
				_serviceDate = service.ServiceDue;
				_serviceTag = service.Tag.Title;
				_addtionalInfo = service.AdditionalInfos.Select(o => o.Comment).ToList();
				_addtionalInfoRequests = service.AdditionalInfoRequests.Select(o => o.Comment).ToList();
				_isCancelled = service.IsCancelled;
				_isAccepted = service.AcceptedBid != null && !service.AcceptedBid.IsCancelled;
			}
			else
			{
				_addtionalInfo = new List<string>();
				_addtionalInfoRequests = new List<string>();
			}
			
		}


		public bool IsAccepted { get { return _isAccepted; } }
		public Guid ServiceId { get { return _serviceId; } }

		[Required]
		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
		[Display(Name = "Bidding Completion Date")]
		public DateTime BiddingCompletionDate { get{ return _biddingCompletionDate; } }

		[Required]
		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
		[Display(Name = "Service Date")]
		public DateTime ServiceDate { get{ return _serviceDate; } }

		[Display(Name = "Reference")]
		public string Reference {get { return _reference; } }

		[Required]
		[Display(Name = "Description")]
		public string Description {get { return _description; } }


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


		public bool IsCancelled { get { return _isCancelled; } }
	}
}