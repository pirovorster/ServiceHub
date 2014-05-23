using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace AnonoMight.Website
{
	public class PostDefinitionCommand 
	{
		public string SiteName { get; set; }
		public string Title { get; set; }
		public string Text { get; set; }
	}
}