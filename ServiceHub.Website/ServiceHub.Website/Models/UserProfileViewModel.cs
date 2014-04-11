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

			Locations = new List<int>();
		}

		public UserProfileViewModel(Model.User user)
		{

			if (user != null)
			{
				Name = user.Name;
				ContactNumber = user.ContactNumber;
				About = user.About;
				LogoData = user.AspNetUser.UserProfileLogo == null ? null : user.AspNetUser.UserProfileLogo.LogoData;
				Tags = string.Join(",", user.Tags.Select(o => o.Title));
				Locations = user.Locations.Select(o => o.Id).ToList();
			}
			else
			{
				Locations = new List<int>();

			}
		}

		[Required]
		[Display(Name = "Public Profile?")]
		public bool IsPublic { get; set; }

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
		[RequiredIf("IsPublic", true, ErrorMessage = "You need to add at least one area if you want to be viewed publically.")]
		public List<int> Locations { get; set; }

		[Display(Name = "Service Tags")]
		[RequiredIf("IsPublic", true, ErrorMessage = "You need to add at least one tag if you want to be viewed publically.")]
		public string Tags { get; set; }
	}
}