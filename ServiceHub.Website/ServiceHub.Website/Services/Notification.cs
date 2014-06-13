using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceHub.Website
{
	public sealed class Notification
	{
		private readonly string _link;
		private readonly string _text;

		internal Notification(string link, string text)
		{
			_link = link;
			_text = text;
		}

		public string Link { get { return _link; } }
		public string Text { get { return _text; } }
	}
}