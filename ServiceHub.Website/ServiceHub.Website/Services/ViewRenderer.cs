using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ServiceHub.Website
{
	public static class ViewRenderer
	{

		private static T CreateController<T>()
			where T : Controller, new()
		{
			// create a disconnected controller instance
			T controller = new T();

			// get context wrapper from HttpContext if available
			HttpContextBase wrapper;
			if (System.Web.HttpContext.Current != null)
				wrapper = new HttpContextWrapper(System.Web.HttpContext.Current);
			else
				throw new InvalidOperationException(
					"Can't create Controller Context if no " +
					"active HttpContext instance is available.");


			RouteData routeData = new RouteData();

			// add the controller routing if not existing
			if (!routeData.Values.ContainsKey("controller") &&
				!routeData.Values.ContainsKey("Controller"))
				routeData.Values.Add("controller",
									 controller.GetType()
											   .Name.ToLower().Replace("controller", ""));

			controller.ControllerContext = new ControllerContext(wrapper, routeData, controller);
			return controller;
		}

		internal static string RenderViewToString(
									string viewPath,
									object model = null,
									bool partial = false)
		{
			GenericController genericController = CreateController<GenericController>();

			// first find the ViewEngine for this view
			ViewEngineResult viewEngineResult = null;
			if (partial)
				viewEngineResult = ViewEngines.Engines.FindPartialView(genericController.ControllerContext, viewPath);
			else
				viewEngineResult = ViewEngines.Engines.FindView(genericController.ControllerContext, viewPath, null);

			if (viewEngineResult == null)
				throw new FileNotFoundException("View cannot be found.");

			// get the view and attach the model to view data
			var view = viewEngineResult.View;
			genericController.ControllerContext.Controller.ViewData.Model = model;

			string result = null;

			using (var sw = new StringWriter())
			{
				var ctx = new ViewContext(genericController.ControllerContext, view,
											genericController.ControllerContext.Controller.ViewData,
											genericController.ControllerContext.Controller.TempData,
											sw);
				view.Render(ctx, sw);
				result = sw.ToString();
			}

			return result;
		}


		public class GenericController : Controller
		{ }
	}


}