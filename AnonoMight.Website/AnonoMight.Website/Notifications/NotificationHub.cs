using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace AnonoMight.Website
{
	public class NotificationHub : Hub
	{
		private readonly DataPersistor _dataPersistor = new DataPersistor();
		private readonly Guid _secret = Guid.Parse("0d2da649-af58-4eae-90a2-7c452478b0e1");
		private readonly FacebookInfo facebookInfo = new FacebookInfo();
		public NotificationHub()
		{
			
		}
		public void PostTweet(PostTweetCommand command)
		{
			Guid id = Guid.NewGuid();
			_dataPersistor.SaveTweet(command, id);
			Clients.Group(command.SiteName).postTweetReceived(new PostedTweetEvent { Id=id, Text = command.Text, Timestamp = DateTime.Now });
		} 

		public void PostFacebookLink(PostFacebookLinkCommand command)
		{
			Guid id = Guid.NewGuid();
			var info = facebookInfo.GetFacebookProfileInfo(command.Url);
			_dataPersistor.SaveFacebookLink(info, command.SiteName, id);

			Clients.Group(command.SiteName).postFacebookLinkReceived(new PostedFacebookLinkEvent { Id = id, Name = info.Name, ProfilePicture = info.FacebookImage, Timestamp = DateTime.Now });
		}
		public void PostDefinition(PostDefinitionCommand command)
		{
			Guid id = Guid.NewGuid();
			_dataPersistor.SaveDefinition(command, id);
			Clients.Group(command.SiteName).postDefinitionReceived(new PostedDefinitionEvent { Id = id, Title = command.Title, Text = command.Text, Timestamp = DateTime.Now });
		}

		public void NotifyPostedImage(NotifyPostedImageCommand command)
		{
			if (command.Secret == _secret)
			{
				Clients.Group(command.SiteName).postImageReceived(new PostedImageEvent { Id =command.Id, Image = command.Image, Timestamp = command.Timestamp });
			}
		}

		public Task AddGroup(JoinCommand command)
		{
			return Groups.Add(Context.ConnectionId, command.SiteName);
		}

		
	}
}