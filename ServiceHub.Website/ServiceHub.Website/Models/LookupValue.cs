using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceHub.Website.Models
{
	public sealed class LookupValue
	{
		private readonly Guid _id;
		private readonly string _value;

		public LookupValue(Guid id, string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				throw new ArgumentException(" value cannot be null or whitespace");

			_id = id;
			_value = value;
		}

		public Guid Id { get { return _id; } }

		public string Value { get { return _value; } }
	}
}