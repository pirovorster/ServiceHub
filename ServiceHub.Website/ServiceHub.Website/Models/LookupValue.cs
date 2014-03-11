using ServiceHub.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceHub.Website.Models
{
	public sealed class MyServiceItem
	{
		private readonly Guid _serviceId;
		private readonly decimal _amount;
		private readonly string _reference;
		private readonly string _status;
		public MyServiceItem(Service service)
		{
			if (service==null)
				throw new ArgumentException("service cannot be null or whitespace");

			_serviceId = id;
			_amount = value;
			_reference = "";
			_status = "";
		}

		public object Id { get { return _id; } }

		public string Value { get { return _value; } }
	}
}