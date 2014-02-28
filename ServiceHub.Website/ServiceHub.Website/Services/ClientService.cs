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
using System.Globalization;
using PagedList;
using WebMatrix.WebData;

namespace ServiceHub.Website
{
	public sealed class ClientService
	{
		

		public void SaveUserProfile(UserProfileViewModel userProfileViewModel)
		{
			if (userProfileViewModel == null)
				throw new ArgumentNullException("userProfileViewModel");

			int userId = WebSecurity.CurrentUserId;

			using (ServiceHubEntities serviceHubEntities = new ServiceHubEntities())
			{
				Client client = serviceHubEntities.Clients.SingleOrDefault(o => o.UserId == userId);
				if (client == null)
				{
					client = new Client();
					serviceHubEntities.Clients.Add(client);
					client.UserId = userId;
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

			int userId = WebSecurity.CurrentUserId;
			using (ServiceHubEntities serviceHubEntities = new ServiceHubEntities())
			{

				Client client = serviceHubEntities.Clients.SingleOrDefault(o => o.UserId == userId);
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


		public void PostService(PostServiceViewModel postServiceViewModel)
		{

			int userId = WebSecurity.CurrentUserId;
			using (ServiceHubEntities serviceHubEntities = new ServiceHubEntities())
			{
				Client client = serviceHubEntities.Clients.SingleOrDefault(o => o.UserId == userId);
				Tag tag = serviceHubEntities.Tags.SingleOrDefault(o => o.Id == postServiceViewModel.ServiceTagId);
				Location location = serviceHubEntities.Locations.SingleOrDefault(o => o.Id == postServiceViewModel.LocationId);

				Service service = new Service();
				service.Description = postServiceViewModel.Description;
				service.Client = client;
				service.BiddingCompletionDate = postServiceViewModel.BiddingCompletionDate.Value;
				service.ServiceDue = postServiceViewModel.ServiceDate.Value;
				service.Tag = tag;
				service.Location = location;
				service.TimeStamp = DateTime.Now;
				service.IsCancelled = false;
				postServiceViewModel.Reference = service.Reference = string.Format(CultureInfo.InvariantCulture, "S{0}", DateTime.Now.Ticks);

				serviceHubEntities.Services.Add(service);

				serviceHubEntities.SaveChanges();
			}

		}

		internal IPagedList<Service> GetServicesPage(int page, int itemsPerPage, IEnumerable<int> locations, IEnumerable<Guid> tags, string searchString)
		{
			using (ServiceHubEntities serviceHubEntities = new ServiceHubEntities())
			{
				IQueryable<Service> services = serviceHubEntities.Services
					.Include("Client")
					.Include("Tag")
					.Include("Location")
					.Where(o => !o.IsCancelled);

				if (locations.Count() > 0 && tags.Count() > 0)
					services = services.Where(o => locations.Contains(o.LocationId) || tags.Contains(o.TagId));
				else if (locations.Count() > 0)
					services = services.Where(o => locations.Contains(o.LocationId));
				else if (tags.Count() > 0)
					services = services.Where(o => tags.Contains(o.TagId));

				if (!string.IsNullOrWhiteSpace(searchString))
				{
					string searchStringUpperCase = searchString.ToUpper();
					services = services.Where(o => o.Description.ToUpper().Contains(searchStringUpperCase));
				}

				return services
					.OrderByDescending(o => o.TimeStamp)
					.ToPagedList(page, itemsPerPage);
			}
		}

		internal ServiceBidViewModel GetService(Guid serviceId)
		{
			int userId = WebSecurity.CurrentUserId;
			using (ServiceHubEntities serviceHubEntities = new ServiceHubEntities())
			{
				Service service = serviceHubEntities.Services
					.Include("Client")
					.Include("Tag")
					.Include("Location")
					.Where(o => !o.IsCancelled)
					.SingleOrDefault(o => o.Id == serviceId);

				decimal highestBid = 
					new List<decimal>
					{
						0
					}.Concat(
					service
					.Bids
					.Where(o => !o.IsCancelled)
					.GroupBy(o => o.ServiceProviderId)
					.Select(o => o.OrderByDescending(i => i.TimeStamp).FirstOrDefault())
					.Where(o => o != null)
					.Select(o=>o.Amount))
					.Max(o => o);

				decimal serviceProviderCurrentBid = new List<decimal>
					{
						0
					}.Concat(service
					.Bids
					.Where(o=>o.ServiceProvider.Client.UserId == userId && !o.IsCancelled)
					.OrderByDescending(i => i.TimeStamp)
					.Select(o=>o.Amount))
					.Max(o=>o);

				if (service != null)
					return new ServiceBidViewModel
					{
						ServiceProviderId = serviceHubEntities.Clients.Single(o=>o.UserId ==userId).Id,
						ServiceId = serviceId,
						HighestBid = highestBid,
						 ServiceProviderCurrentBid = serviceProviderCurrentBid,
						BiddingCompletionDate = service.BiddingCompletionDate,
						Description = service.Description,
						Location = service.Location.Name,
						Reference = service.Reference,
						ServiceDate = service.ServiceDue,
						ServiceTag = service.Tag.Title,
						AddtionalInfo = service.AdditionalInfos.Select(o => o.Comment).ToList(),
						AddtionalInfoRequests = service.AdditionalInfoRequests.Select(o => o.Comment).ToList(),

					};
				else
					return new ServiceBidViewModel();

			}
		}
	}
}