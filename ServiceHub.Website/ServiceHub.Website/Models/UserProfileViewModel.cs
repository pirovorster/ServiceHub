using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceHub.Website.Models
{
	public sealed class UserProfileViewModel
	{
		public UserProfileViewModel()
		{
			Locations = new List<Guid>();
		}

		[Required]
		[Display(Name = "Do you also which to bid on services?")]
		public bool IsServiceProvider { get; set; }

		[Required]
		[Display(Name = "Name")]
		public string Name { get; set; }

		[Required]
		[Display(Name = "Contact Number")]
		public string ContactNumber { get; set; }

		[Display(Name = "About")]
		public string About { get; set; }

		[Display(Name = "Logo")]
		public byte[] LogoData { get; set; }

		[Display(Name = "Working Areas")]
		[RequiredIf("IsServiceProvider", true, ErrorMessage = "You need to add at least one area.")]
		public List<Guid> Locations { get; set; }

		[Display(Name = "Tags")]
		[RequiredIf("IsServiceProvider", true, ErrorMessage = "You need to add at least one tag.")]
		public string Tags { get; set; }
	}
}