using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using ServiceHub.Website.Models;

namespace ServiceHub.Website
{
	public static class AuthConfig
	{
		public static void RegisterAuth()
		{
			// To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
			// you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

			//OAuthWebSecurity.RegisterMicrosoftClient(
			//	clientId: "000000004C114725",
			//	clientSecret: "DXZQlo30yf-YeAawTsSFTWsqEnGuYcGS");

			OAuthWebSecurity.RegisterTwitterClient(
				consumerKey: "vAmDUWJX1AMBgokwjllOdR5l5 ",
				consumerSecret: "EIAckdhha1PcKWD6UtubvDULZr4c1Ii1ugldm24X2ATu6d2l01");

			OAuthWebSecurity.RegisterFacebookClient(
				appId: "489374234501612",
				appSecret: "672aebbf6ec37823fa9e2163bd0832e9");

			OAuthWebSecurity.RegisterGoogleClient();
		}
	}
}
