using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace AnonoMight.Website
{
	public class PostedTweetEvent
	{
		public Guid Id { get; set; }
		public string Text { get; set; }
		public DateTime Timestamp { get; set; }
	}
}