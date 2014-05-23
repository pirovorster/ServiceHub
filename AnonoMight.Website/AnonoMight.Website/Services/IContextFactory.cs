using AnonoMight.Model;
using System;
namespace AnonoMight.Website.Services
{
	interface IContextFactory
	{
		AnonoMightEntities GetContext(string siteName);
	}
}
