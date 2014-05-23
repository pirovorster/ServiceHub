using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceHub.Website.Models
{
	public sealed class FinalServiceProviderBidViewModel
	{
		private readonly Guid _userId;

		private readonly decimal _bid;
		private readonly double _rating;
		private readonly string _name;
		private readonly string _contactNumber;
		private readonly string _about;
		private readonly string _email;
		
		private readonly List<string> _locations;
		private readonly List<string> _tags;
		private readonly bool _isAccepted;

		public FinalServiceProviderBidViewModel(Model.Bid bid)
		{
			// TODO: Complete member initialization
			_userId = bid.UserId;
			_bid = bid.Amount;
			_rating = bid.User.Rating;
			_name = bid.User.Name;
			_contactNumber = bid.User.ContactNumber;
			_email =bid.User.AspNetUser.UserName;
			_about = bid.User.About;
			_locations = bid.User.Locations.Select(o => o.Name).ToList();
			_tags = bid.User.Tags.Select(o => o.Title).ToList();
			_isAccepted = bid.AcceptedBid != null && !bid.AcceptedBid.IsCancelled;
		}
		public Guid UserId { get { return _userId; } }

		[Display(Name = "Lowest Bid")]
		public decimal Bid { get { return _bid; } }

		[Display(Name = "Rating")]
		public double Rating { get { return _rating; } }

		[Display(Name = "Email")]
		public string Email { get { return _email; } }


		[Display(Name = "Name")]
		public string Name { get { return _name; } }

		[Display(Name = "Contact Number")]
		public string ContactNumber { get { return _contactNumber; } }

		[Display(Name = "About")]
		public string About { get { return _about; } }

		[Display(Name = "Working Areas")]
		public List<string> Locations { get { return _locations; } }

		[Display(Name = "Service Tags")]
		public List<string> Tags { get { return _tags; } }

		public bool IsAccepted { get { return _isAccepted; } }


	}
}