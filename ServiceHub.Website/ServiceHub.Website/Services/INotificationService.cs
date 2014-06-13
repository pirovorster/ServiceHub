using System;
namespace ServiceHub.Website
{
	public interface INotificationService
	{
		void SendBidAcceptedNotifications(Guid serviceId);
		void SendBiddingCompletedNotifications(Guid serviceId);
		void SendNewLowestBidNotifications(Guid serviceId);
		void SendServiceAmmendedNotifications(Guid serviceId);
		void SendServicePostedNotifications(Guid serviceId);
	}
}
