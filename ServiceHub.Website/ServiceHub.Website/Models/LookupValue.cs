using ServiceHub.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceHub.Website.Models
{
	public sealed class LookupValue
	{
		private readonly object _id;
		private readonly string _value;
		public LookupValue(object id, string value)
		{
			if (id == null)
				throw new ArgumentNullException("id");


			if (string.IsNullOrWhiteSpace(value))
				throw new ArgumentNullException("value");

			_id = id;
			_value = value;
		}

		public object Id { get { return _id; } }

		public string Value { get { return _value; } }
	}
}