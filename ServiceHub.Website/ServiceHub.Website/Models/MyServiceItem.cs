using ServiceHub.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceHub.Website.Models
{
	public sealed class MyServiceItem
	{
		private readonly Guid _serviceId;
		private readonly decimal _lowestBid;
		private readonly string _reference;
		private readonly string _status;
		private readonly bool _isCompleted;

		private readonly DateTime _estimatedServiceDue;
		private readonly DateTime _biddingCompletion;
		public MyServiceItem(Service service)
		{
			if (service == null)
				throw new ArgumentException("service cannot be null or whitespace");

			_serviceId = service.Id;

			Bid lowestBid = service.Bids
			.Where(o => !o.IsCancelled)
			.OrderBy(o => o.Amount)
			.FirstOrDefault();

			_lowestBid = lowestBid == null ? 0 : lowestBid.Amount;
			_reference = service.Reference;
			_isCompleted = false;

			if (service.IsCancelled)
			{
				_isCompleted = true;
				_status = "Cancelled";
			}
			else if (service.AcceptedBid != null && !service.AcceptedBid.IsCancelled)
			{
				_isCompleted = true;
				_status = "Accepted";
			}
			else if (service.BiddingCompletionDate < DateTime.Now)
			{
				_status = "Bidding Completed";
				_isCompleted = true;
			}
			else
				_status = "Bidding In Progress";


			_estimatedServiceDue = service.ServiceDue;
			_biddingCompletion = service.BiddingCompletionDate;

		}

		public Guid ServiceId { get { return _serviceId; } }


		[DisplayFormat(DataFormatString = "{0:c}")]
		public decimal LowestBid { get { return _lowestBid; } }


		public string Reference { get { return _reference; } }
		public string Status { get { return _status; } }
		public bool IsCompleted { get { return _isCompleted; } }

		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
		public DateTime ServiceDue { get { return _estimatedServiceDue; } }

		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
		public DateTime BiddingCompletionDate { get { return _biddingCompletion; } }



	}
}