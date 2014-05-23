using AnonoMight.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AnonoMight.Website.Services
{
	internal sealed class TweetsContextFactory : IContextFactory
	{
		public AnonoMightEntities GetContext(string siteName)
		{
			SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AnonoMightConnection"].ConnectionString);
			DbModelBuilder builder = new DbModelBuilder(DbModelBuilderVersion.V6_0);
			builder.Configurations.Add(new EntityTypeConfiguration<Tweet>());
			builder.Entity<Tweet>().ToTable(siteName, "data");
			builder.Configurations.Add(new EntityTypeConfiguration<Site>());
			builder.Entity<Site>().ToTable("Sites", "dbo");
			return new AnonoMightEntities(builder.Build(conn).Compile());
		}
	}
}