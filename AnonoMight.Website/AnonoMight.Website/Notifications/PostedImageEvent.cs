using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace AnonoMight.Website
{
	public class PostedImageEvent
	{
		public Guid Id { get; set; }
		public byte[] Image { get; set; }

		public DateTime Timestamp { get; set; }

	}
}