using ServiceHub.Model;
using ServiceHub.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using System.Drawing;
using System.IO;

namespace ServiceHub.Website
{
	public sealed class ClientService
	{
		private readonly int _userId;
		public ClientService(int userId)
		{
			_userId = userId;
		}

		public void SaveUserProfile(UserProfileViewModel userProfileViewModel)
		{
			if (userProfileViewModel == null)
				throw new ArgumentNullException("userProfileViewModel");

			using (ServiceHubEntities serviceHubEntities = new ServiceHubEntities())
			{
				Client client = serviceHubEntities.Clients.SingleOrDefault(o => o.UserId == _userId);
				if (client == null)
				{
					client = new Client();
					serviceHubEntities.Clients.Add(client);
					client.UserId = _userId;
				}

				client.Name = userProfileViewModel.Name;
				client.ContactNumber = userProfileViewModel.ContactNumber;

				if (userProfileViewModel.IsServiceProvider)
				{
					if (client.ServiceProvider == null)
					{
						client.ServiceProvider = new ServiceProvider();
					}

					client.ServiceProvider.About = userProfileViewModel.About ?? string.Empty;

					if (userProfileViewModel.LogoData != null)
						client.ServiceProvider.Logo = userProfileViewModel.LogoData;

					client.ServiceProvider.IsActive = userProfileViewModel.IsServiceProvider;

					foreach (Location location in client.ServiceProvider.Locations.ToList())
						client.ServiceProvider.Locations.Remove(location);
					foreach (Location location in serviceHubEntities.Locations.Where(o => userProfileViewModel.Locations.Contains(o.Id)).ToList())
						client.ServiceProvider.Locations.Add(location);

					List<string> tagsText = userProfileViewModel.Tags.Split(',').ToList();

					List<Tag> existingTags = serviceHubEntities.Tags.Where(o => tagsText.Contains(o.Title)).ToList();
					foreach (string tagText in tagsText)
					{
						if (!existingTags.Any(o => string.Equals(tagText, o.Title, StringComparison.OrdinalIgnoreCase)))
						{
							existingTags.Add(new Tag { Title = tagText });
						}
					}

					foreach (Tag tag in client.ServiceProvider.Tags.ToList())
						client.ServiceProvider.Tags.Remove(tag);
					foreach (Tag tag in existingTags.ToList())
						client.ServiceProvider.Tags.Add(tag);
				}
				else
				{
					if (client.ServiceProvider != null)
						client.ServiceProvider.IsActive = userProfileViewModel.IsServiceProvider;
				}

				serviceHubEntities.SaveChanges();
			}

		}

		public UserProfileViewModel GetUserProfile()
		{
			UserProfileViewModel userProfileViewModel = new UserProfileViewModel();

			using (ServiceHubEntities serviceHubEntities = new ServiceHubEntities())
			{

				Client client = serviceHubEntities.Clients.SingleOrDefault(o => o.UserId == _userId);
				if (client == null)
					return userProfileViewModel;

				userProfileViewModel.Name = client.Name;
				userProfileViewModel.ContactNumber = client.ContactNumber;

				if (client.ServiceProvider != null)
				{

					if (client.ServiceProvider == null)
					{
						client.ServiceProvider = new ServiceProvider();
					}

					userProfileViewModel.About = client.ServiceProvider.About;
					userProfileViewModel.LogoData = client.ServiceProvider.Logo;
					userProfileViewModel.IsServiceProvider = client.ServiceProvider.IsActive;

					userProfileViewModel.Tags = string.Join(",", client.ServiceProvider.Tags.Select(o => o.Title));
					userProfileViewModel.Locations = client.ServiceProvider.Locations.Select(o => o.Id).ToList();

				}
				else
				{

					userProfileViewModel.IsServiceProvider = false;
				}

				return userProfileViewModel;
			}

		}

		
		public IEnumerable<LookupValue> GetLocations()
		{
			using (ServiceHubEntities serviceHubEntities = new ServiceHubEntities())
			{
				return serviceHubEntities.Locations.ToList().Select(o => new LookupValue(o.Id, o.Name)).ToList().AsReadOnly();
			}
		}

		public IEnumerable<LookupValue> GetTags()
		{
			using (ServiceHubEntities serviceHubEntities = new ServiceHubEntities())
			{
				return serviceHubEntities.Tags.ToList().Select(o => new LookupValue(o.Id, o.Title)).ToList().AsReadOnly();


			}
		}
	}
}