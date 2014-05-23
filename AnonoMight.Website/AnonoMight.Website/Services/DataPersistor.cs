using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using AnonoMight.Website.Services;
using AnonoMight.Model;

namespace AnonoMight.Website
{
	internal sealed class DataPersistor
	{
		internal void SaveTweet(PostTweetCommand command, Guid id)
		{
			TweetsContextFactory contextFactory = new TweetsContextFactory();
			AnonoMightEntities context = contextFactory.GetContext(command.SiteName);
			context.Tweets.Add(new Tweet { Id = id, Text = command.Text, TimeStamp = DateTime.Now, Reports = 0 });
			context.SaveChanges();
		}

		internal void SaveImage(byte[] data, string siteName, Guid id)
		{
			ImagePostsContextFactory contextFactory = new ImagePostsContextFactory();
			AnonoMightEntities context = contextFactory.GetContext(siteName);
			context.ImagePosts.Add(new ImagePost { Id = id, Link = data, TimeStamp = DateTime.Now, Reports = 0 });
			context.SaveChanges();
		}

		internal void SaveDefinition(PostDefinitionCommand command, Guid id)
		{
			DefinitionsContextFactory contextFactory = new DefinitionsContextFactory();
			AnonoMightEntities context = contextFactory.GetContext(command.SiteName);
			context.Definitions.Add(new Definition { Id = id, Text = command.Text, Name = command.Title, TimeStamp = DateTime.Now, Reports = 0 });
			context.SaveChanges();
		}
		internal void SaveFacebookLink(PostFacebookLinkCommand command, Guid id)
		{
			FacebookProfilesContextFactory contextFactory = new FacebookProfilesContextFactory();
			AnonoMightEntities context = contextFactory.GetContext(command.SiteName);
			context.FacebookProfiles.Add(new FacebookProfile { Id = id, Link = command.Url, TimeStamp = DateTime.Now, Reports = 0 });
			context.SaveChanges();
		}

		internal void SaveFacebookLink(FacebookProfileInfo facebookProfileInfo, string siteName, Guid id)
		{
			FacebookProfilesContextFactory contextFactory = new FacebookProfilesContextFactory();
			AnonoMightEntities context = contextFactory.GetContext(siteName);
			context.FacebookProfiles.Add(new FacebookProfile { Id = id, Name = facebookProfileInfo.Name, ProfileImage = facebookProfileInfo.FacebookImage, Link = facebookProfileInfo.Link, TimeStamp = DateTime.Now, Reports = 0 });
			context.SaveChanges();
		}
	}
}