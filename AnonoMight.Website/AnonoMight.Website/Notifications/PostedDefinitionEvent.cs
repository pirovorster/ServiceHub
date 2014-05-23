using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace AnonoMight.Website
{
	public class PostedDefinitionEvent 
	{
		public Guid Id { get; set; }
		public string Text { get; set; }
		public string Title { get; set; }
		public DateTime Timestamp { get; set; }
	}
}