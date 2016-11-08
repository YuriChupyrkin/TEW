using System.Web;
using System.Web.Optimization;

namespace TewCloud
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

      bundles.Add(new ScriptBundle("~/bundles/tewScripts").Include(
                  "~/Scripts/TewScripts/TewInfo.js"));

      // Use the development version of Modernizr to develop with and learn from. Then, when you're
      // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
      bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                  "~/Scripts/modernizr-*"));

      bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

      bundles.Add(new ScriptBundle("~/bundles/angular2").Include(
                 "~/Scripts/AngularJS/node_modules/core-js/client/shim.min.js",
                 "~/Scripts/AngularJS/node_modules/zone.js/dist/zone.js",
                 "~/Scripts/AngularJS/node_modules/reflect-metadata/temp/Reflect.js",
                 "~/Scripts/AngularJS/node_modules/systemjs/dist/system.src.js"));

      bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-responsive.css",
                "~/Content/Learning.css",
                "~/Content/site.css"));

      // Set EnableOptimizations to false for debugging. For more information,
      // visit http://go.microsoft.com/fwlink/?LinkId=301862
      BundleTable.EnableOptimizations = true;
    }
  }
}
