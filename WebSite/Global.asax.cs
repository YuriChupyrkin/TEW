using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebSite
{
  public class WebApiApplication : System.Web.HttpApplication
  {
    protected void Application_Start()
    {
      AreaRegistration.RegisterAllAreas();
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);

      AutofacModule.RegisterAutoFac();

      //var repositoryFactory = DependencyResolver.Current.GetService <IRepositoryFactory>();
      //repositoryFactory.EnRuWordsRepository.AddTranslate("TEW", "TEW", "TEW", 1);
      //((IUnitOfWork)repositoryFactory).Commit();
    }
  }
}
