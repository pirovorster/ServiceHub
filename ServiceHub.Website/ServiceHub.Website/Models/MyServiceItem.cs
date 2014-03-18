using ServiceHub.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceHub.Website.Models
{
	public sealed class MyBidItem
	{
		private readonly Guid _serviceId;
		private readonly decimal _myHighestBid;
		private readonly decimal _highestBid;
		private readonly string _reference;
		private readonly string _status;
		private readonly bool _isCompleted;

		private readonly DateTime _estimatedServiceDue;
		private readonly DateTime _biddingCompletion;
		public MyBidItem(Bid service, Guid userId)
		{
			if (service == null)
				throw new ArgumentException("service cannot be null or whitespace");

			_serviceId = service.Id;
			_highestBid = service.Bids
				.Where(o => !o.IsCancelled)
				.GroupBy(o => o.UserId)
				.Select(o => o.OrderByDescending(i => i.TimeStamp).FirstOrDefault())
				.Where(o => o != null)
				.Max(o => o.Amount);
			_myHighestBid = service.Bids
				.Where(o => !o.IsCancelled && o.UserId == userId)
				.OrderByDescending(o => o.TimeStamp)
				.First().Amount;

			_reference = service.Reference;
			_isCompleted = service.BiddingCompletionDate<DateTime.Now;

			if (service.AcceptedBid!=null && !service.AcceptedBid.IsCancelled)
			{
				if (service.AcceptedBid.Bid.UserId == userId)
					_status = "Successful";
				else
					_status = "Unsuccessful";
			}
			else
				_status = "To be accepted!";

			_estimatedServiceDue = service.ServiceDue;
			_biddingCompletion = service.BiddingCompletionDate;
			
		}

		public Guid ServiceId { get { return _serviceId; } }


		[DisplayFormat(DataFormatString = "{0:c}")]
		public decimal MyHighestBid { get { return _myHighestBid; } }


		[DisplayFormat(DataFormatString = "{0:c}")]
		public decimal HighestBid { get { return _highestBid; } }
		public string Reference { get { return _reference; } }
		public string Status { get { return _status; } }
		public bool IsCompleted { get { return _isCompleted; } }

		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
		public DateTime ServiceDue { get { return _estimatedServiceDue; } }

		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
		public DateTime BiddingCompletionDate { get { return _biddingCompletion; } }

		
	
	}
}