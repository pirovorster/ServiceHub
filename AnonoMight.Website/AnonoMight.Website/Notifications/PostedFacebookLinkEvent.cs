using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace AnonoMight.Website
{
	public class PostedFacebookLinkEvent
	{
		public Guid Id { get; set; }
		public byte[] ProfilePicture { get; set; }
		public string Name { get; set; }

		public DateTime Timestamp { get; set; }
	}
}