using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace AnonoMight.Website
{
	public class PostFacebookLinkCommand 
	{
		public string SiteName { get; set; }
		public string Url { get; set; }
	}
}