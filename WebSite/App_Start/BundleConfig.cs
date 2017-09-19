using System.Web.Optimization;

namespace WebSite
{
  public class BundleConfig
  {
    // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
    public static void RegisterBundles(BundleCollection bundles)
    {
      bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                  "~/Scripts/jquery-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryValidation").Include(
									"~/Scripts/jquery.validate.js",
									"~/Scripts/jquery.validate-vsdoc.js",
									"~/Scripts/jquery.validate.unobtrusive.js"));

      // Use the development version of Modernizr to develop with and learn from. Then, when you're
      // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
      bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                  "~/Scripts/modernizr-*"));

      bundles.Add(new ScriptBundle("~/bundles/angular2").Include(
                 "~/StaticContent/node_modules/core-js/client/shim.min.js",
                 "~/StaticContent/node_modules/zone.js/dist/zone.js",
                 "~/StaticContent/node_modules/reflect-metadata/temp/Reflect.js",
                 "~/StaticContent/node_modules/systemjs/dist/system.src.js"));

      bundles.Add(new StyleBundle("~/Content/awesome").Include(
                "~/Content/font-awesome.css"));

      bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/Site.css"));

      // Set EnableOptimizations to false for debugging. For more information,
      // visit http://go.microsoft.com/fwlink/?LinkId=301862
      BundleTable.EnableOptimizations = true;
    }
  }
}
