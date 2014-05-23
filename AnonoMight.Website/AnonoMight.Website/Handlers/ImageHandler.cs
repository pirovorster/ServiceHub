using AnonoMight.Model;
using AnonoMight.Website.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnonoMight.Website.Handlers
{
	public class ImageHandler : IHttpHandler
	{
		private readonly ImagePostsContextFactory _imagePostsContextFactory = new ImagePostsContextFactory();
		
		public bool IsReusable
		{
			get { return true; }
		}

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "image/jpg";
			string[] tokens = context.Request.RawUrl.Split('/');
			string siteName = tokens.Reverse().Skip(2).First();
			AnonoMightEntities  anonoMightEntities = _imagePostsContextFactory.GetContext(siteName);
			Guid id =Guid.Parse(tokens.Last());
			byte[] data = anonoMightEntities.ImagePosts.Single(o => o.Id == id).Link;
			context.Response.OutputStream.Write(data, 0, data.Length);
		}
	}
}