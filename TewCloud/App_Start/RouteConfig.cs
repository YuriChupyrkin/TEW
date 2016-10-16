using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace TewCloud
{
  public class RouteConfig
  {
    public static void RegisterRoutes(RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

      routes.MapHttpRoute("Api default without id", "api/{controller}");
      routes.MapHttpRoute("Api default", "api/{controller}/{id}", new { id = UrlParameter.Optional });

      routes.MapRoute(
          name: "Default",
          url: "{controller}/{action}/{id}",
          defaults: new { controller = "Tew", action = "Learning", id = UrlParameter.Optional }
      );

      GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
      GlobalConfiguration.Configuration.Formatters.JsonFormatter
          .MediaTypeMappings.Add(new QueryStringMapping("json", "true", "application/json"));

    }
  }
}
