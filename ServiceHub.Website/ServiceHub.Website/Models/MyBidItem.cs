﻿using ServiceHub.Model;
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
		private readonly decimal _myLowestBid;
		private readonly decimal _lowestBid;
		private readonly string _reference;
		private readonly string _status;
		private readonly bool _isCompleted;

		private readonly DateTime _estimatedServiceDue;
		private readonly DateTime _biddingCompletion;
		public MyBidItem(Service service, Guid userId)
		{
			if (service == null)
				throw new ArgumentException("service cannot be null or whitespace");

			_serviceId = service.Id;
			_lowestBid = service
				.LatestBids()
				.Min(o => o.Amount);

			_myLowestBid = service.LatestBidForUser(userId).Amount;

			_reference = service.Reference;
			_isCompleted = service.BiddingCompletionDate<DateTime.Now;

			if (service.AcceptedBid.IsCancelled)
				_status = "Cancelled";
			 else if (service.AcceptedBid!=null )
			{
				if (service.AcceptedBid.Bid.UserId == userId)
					_status = "Successful";
				else
					_status = "Unsuccessful";
			}
			else
				_status = "To be accepted!";

			//To do: cancelled service

			_estimatedServiceDue = service.ServiceDue;
			_biddingCompletion = service.BiddingCompletionDate;
			
		}

		public Guid ServiceId { get { return _serviceId; } }


		[DisplayFormat(DataFormatString = "{0:c}")]
		public decimal MyLowestBid { get { return _myLowestBid; } }


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