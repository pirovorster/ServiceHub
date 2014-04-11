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
		private readonly List<string> _locations;
		private readonly List<string> _tags;

		public FinalServiceProviderBidViewModel(Model.Bid bid)
		{
			// TODO: Complete member initialization
			_userId = bid.UserId;
			_bid = bid.Amount;
			_rating = bid.User.Rating;
			_name = bid.User.Name;
			_contactNumber = bid.User.ContactNumber;
			_about = bid.User.About;
			_locations = bid.User.Locations.Select(o => o.Name).ToList();
			_tags = bid.User.Tags.Select(o => o.Title).ToList();
		}
		public Guid UserId { get; set; }

		[Display(Name = "Lowest Bid")]
		public decimal Bid { get; set; }

		[Display(Name = "Rating")]
		public decimal Rating { get; set; }

		[Display(Name = "Name")]
		public string Name { get; set; }

		[Display(Name = "Contact Number")]
		public string ContactNumber { get; set; }

		[Display(Name = "About")]
		public string About { get; set; }

		[Display(Name = "Working Areas")]
		public List<string> Locations { get; set; }

		[Display(Name = "Service Tags")]
		public List<string> Tags { get; set; }
	}
}