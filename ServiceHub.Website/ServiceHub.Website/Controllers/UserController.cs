using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using ServiceHub.Website.Filters;
using ServiceHub.Website.Models;
using System.Web.Script.Serialization;

namespace ServiceHub.Website.Controllers
{
	[Authorize]
	public class UserController : Controller
	{
		public ActionResult UserProfile()
		{
			JavaScriptSerializer serializer = new JavaScriptSerializer();

			ViewBag.Tags =ViewBag.Locations = new MultiSelectList(new List<LookupValue>
			{
				new LookupValue(Guid.NewGuid(), "Value one"),
				new LookupValue(Guid.NewGuid(), "Value two"),
				new LookupValue(Guid.NewGuid(), "Value three"),
				new LookupValue(Guid.NewGuid(), "Value four")
			}, "Id", "Value");

			//ViewBag.Tags = serializer.Serialize(new List<string>
			//{
			//	"Value one",
			//	"Value two",
			//	"Value three"

			//});
			return View();
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult UserProfile(UserProfileViewModel userProfileViewModel, HttpPostedFileBase image)
		{
			if (ModelState.IsValid)
			{
				if (image != null)
				{
					//product.ImageMimeType = image.ContentType;
					//product.ImageData = new byte[image.ContentLength];
					//image.InputStream.Read(product.ImageData, 0, image.ContentLength);
				}
				//productsRepository.SaveProduct(product);
				//TempData["message"] = product.Name + " has been saved.";
				return View();
			}
			else // Validation error, so redisplay same view
				return View(userProfileViewModel);
		}

		
	}
}
