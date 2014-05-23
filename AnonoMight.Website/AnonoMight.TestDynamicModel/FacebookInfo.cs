using AnonoMight.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Web;

namespace AnonoMight.Website
{
	internal class FacebookInfo
	{
		internal FacebookProfileInfo GetFacebookProfileInfo(string profileUrl)
		{
			string infoUrl = profileUrl.Split('?').First().Replace("www", "graph");
			string imageUrl = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", infoUrl, "picture?type=large");
			try
			{
				HttpWebRequest infoRequest = WebRequest.Create(infoUrl) as HttpWebRequest;
				using (HttpWebResponse infoRespose = infoRequest.GetResponse() as HttpWebResponse)
				{
					if (infoRespose.StatusCode != HttpStatusCode.OK)
						throw new Exception(String.Format(
						"Server error (HTTP {0}: {1}).",
						infoRespose.StatusCode,
						infoRespose.StatusDescription));
					DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(FacebookProfileInfo));
					object objResponse = jsonSerializer.ReadObject(infoRespose.GetResponseStream());
					FacebookProfileInfo facebookProfileInfo
					= objResponse as FacebookProfileInfo;

					HttpWebRequest imageRequest = (HttpWebRequest)WebRequest.Create(imageUrl);
					imageRequest.Credentials = CredentialCache.DefaultCredentials;
					imageRequest.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, " +
						"application/vnd.ms-excel, application/vnd.ms-powerpoint, " +
						"application/msword, */*";
					WebResponse imageResponse = imageRequest.GetResponse();

					using (MemoryStream ms = new MemoryStream())
					{
						imageResponse.GetResponseStream().CopyTo(ms);
						facebookProfileInfo.FacebookImage = ms.ToArray();
					}
					return facebookProfileInfo;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return null;
			}
		}




		[DataContract]
		internal class FacebookProfileInfo
		{
			[DataMember(Name = "id")]
			public string Id { get; set; }
			[DataMember(Name = "first_name")]
			public string FirstName { get; set; }
			[DataMember(Name = "gender")]
			public string Gender { get; set; }
			[DataMember(Name = "link")]
			public string Link { get; set; }
			[DataMember(Name = "locale")]
			public string Locale { get; set; }
			[DataMember(Name = "name")]
			public string Name { get; set; }
			[DataMember(Name = "username")]
			public string Username { get; set; }

			public byte[] FacebookImage { get; set; }

		}
	}
}