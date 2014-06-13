using ServiceHub.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServiceHub.Website
{
	internal class NotificationService : ServiceHub.Website.INotificationService
	{
		private readonly ServiceHubEntities _serviceHubEntities;

		public NotificationService(ServiceHubEntities serviceHubEntities)
		{
			_serviceHubEntities = serviceHubEntities;
		}
		
		public void SendServicePostedNotifications(Guid serviceId)
		{
			Service service = _serviceHubEntities.Services.Single(o => o.Id == serviceId);
			int locationId = service.LocationId;
			Guid serviceTag = service.TagId;
			IEnumerable<User> users = _serviceHubEntities.GetUserRelatedToServiceTagAndLocation(locationId, serviceTag);
			SendNotification(serviceId, users,"A service related to your subscribed business has been posted in your area.");
			
		}

		private static void SendNotification(Guid serviceId, IEnumerable<User> users,string message)
		{

			UrlHelper url = new UrlHelper(HttpContext.Current.Request.RequestContext);
			EmailSender email = new EmailSender();
			string serviceUrl = url.Action("Service", "Client", new { serviceId = serviceId }, "http");

			string html = ViewRenderer.RenderViewToString(
			"~/views/email/NotificationTemplate.cshtml",
			new Notification[] { new Notification(serviceUrl, message) }, true);

			email.SendMail(users.Select(o => o.AspNetUser.UserName), "Notification", html);
		}

		public void SendServiceAmmendedNotifications(Guid serviceId)
		{
			Service service = _serviceHubEntities.Services.Single(o => o.Id == serviceId);
			IEnumerable<User> users = service.Bids.Select(o => o.User);
			SendNotification(serviceId, users, "A service you have bid on has been modified.");
			
		}

		public void SendNewLowestBidNotifications(Guid serviceId)
		{
			Service service = _serviceHubEntities.Services.Single(o => o.Id == serviceId);
			IEnumerable<User> users = service.Bids.Select(o => o.User);

			SendNotification(serviceId, users, "A service you have bid on have a new  lowest bid.");
		}

		public void SendBidAcceptedNotifications(Guid serviceId)
		{
			Service service = _serviceHubEntities.Services.Single(o => o.Id == serviceId);
			AcceptedBid acceptedBid = service.AcceptedBid;

			if(acceptedBid!=null && !acceptedBid.IsCancelled)
			{
				User user = acceptedBid.Bid.User;
				SendNotification(serviceId, new User[] { user }, "One of your bids have been accepted by the service requester.");
			}
		}

		public void SendBiddingCompletedNotifications(Guid serviceId)
		{
			Service service = _serviceHubEntities.Services.Single(o => o.Id == serviceId);
			User user = service.User;
			SendNotification(serviceId, new User[] { user }, "Bidding is completed on one of the services you have requested.");

		}

	}


}