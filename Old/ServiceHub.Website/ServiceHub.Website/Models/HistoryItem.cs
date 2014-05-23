using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceHub.Website.Models
{
	public sealed class HistoryItem
	{
		private readonly DateTime _timestamp;
		private readonly string _action;
		private readonly string _description;

		public HistoryItem(DateTime timestamp, string action, string description)
		{
			if (string.IsNullOrWhiteSpace(description))
				throw new ArgumentException("description cannot be null or whitespace");
			
			if (string.IsNullOrWhiteSpace(action))
				throw new ArgumentException("action cannot be null or whitespace");

			_timestamp = timestamp;
			_action = action;
			_description = description;
		}


		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy HH:mm}")]
		public DateTime Timestamp { get { return _timestamp; } }
		public string Action { get { return _action; } }
		public string Description { get { return _description; } }

	}
}