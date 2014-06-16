using System.Web;
using System.Web.Optimization;

namespace ServiceHub.Website
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						
						"~/Scripts/jquery-{version}.js",
						"~/Scripts/jquery.unobtrusive*",
						"~/Scripts/select2.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/jquery.validate*",
						"~/Scripts/MicrosoftAjax.js",
						"~/Scripts/MicrosoftMvcAjax.js",
						"~/Scripts/MicrosoftMvcValidation.js",
						"~/Scripts/mvcfoolproof.unobtrusive.js",
						"~/Scripts/MvcFoolproofJQueryValidation.js",
						"~/Scripts/MvcFoolproofValidation.js"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
					  "~/Scripts/bootstrap.js",
					  "~/Scripts/bootstrap-datepicker.js",
					  "~/Scripts/respond.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
					  //"~/Content/bootstrap.css",
					  "~/Content/bootstrap-theme.css",
					  "~/Content/bootstrap-datepicker.css",
					  "~/Content/site.css",
				"~/Content/custom.css",
				"~/Content/PagedList.css"));

			bundles.Add(new StyleBundle("~/Content/select2/css").Include("~/Content/select2/select2.css"));



		}
	}
}
