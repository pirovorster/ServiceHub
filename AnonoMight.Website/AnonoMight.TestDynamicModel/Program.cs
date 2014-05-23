using AnonoMight.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using AnonoMight.Website;

namespace AnonoMight.TestDynamicModel
{
	class Program
	{
		static void Main(string[] args)
		{
			FacebookInfo facebookInfo = new FacebookInfo();
			facebookInfo.GetFacebookProfileInfo("https://www.facebook.com/sally.bickerton.395?fref=ts");

			//SqlConnection conn = new SqlConnection( ConfigurationManager.ConnectionStrings["AnonoMightConnection"].ConnectionString);
			//Database.SetInitializer<AnonoMightEntities>(null);
			//DbModelBuilder builder = new DbModelBuilder(DbModelBuilderVersion.V6_0);
			//builder.Configurations.Add(new EntityTypeConfiguration<Tweet>());

			//builder.Entity<Tweet>().ToTable("BrainFarts", "data");

			//using (AnonoMightEntities anonoMightEntities2 = new AnonoMightEntities(builder.Build(conn).Compile()))
			//{
			//	var a = anonoMightEntities2.Tweets.ToList();


			//}

			//DbModelBuilder builder2 = new DbModelBuilder(DbModelBuilderVersion.V6_0);
			//builder2.Configurations.Add(new EntityTypeConfiguration<Definition>());
			//builder2.Entity<Definition>().ToTable("CreateReligion", "data");

			//using (AnonoMightEntities anonoMightEntities2 = new AnonoMightEntities(builder.Build(conn).Compile()))
			//{
			//	var a = anonoMightEntities2.Definitions.ToList();
			//}

		}

	}
}

