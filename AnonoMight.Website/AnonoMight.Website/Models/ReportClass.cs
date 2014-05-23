using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnonoMight.Website.Models
{
	public enum ReportClass : byte
	{
		OffTopic = 1,
		Illegal = 2,
		Spam = 3,
		Other = 0
	}
}