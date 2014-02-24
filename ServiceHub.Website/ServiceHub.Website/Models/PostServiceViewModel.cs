using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceHub.Website.Models
{
	public sealed class PostServiceViewModel
	{
		public PostServiceViewModel()
		{
			
		}

		[Required]
		[Display(Name = "Bidding Completion Date")]
		public DateTime? BiddingCompletionDate { get; set; }

		[Required]
		[Display(Name = "Service Date")]
		public DateTime? ServiceDate { get; set; }

		[Display(Name = "Reference")]
		public string Reference { get; set; }

		[Required]
		[Display(Name = "Description")]
		public string Description { get; set; }


		[Display(Name = "Location")]
		[Required]
		public int? LocationId { get; set; }

		[Display(Name = "ServiceTag")]
		[Required]
		public Guid? ServiceTagId { get; set; }
	}
}