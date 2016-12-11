using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Domain.Entities;
using Domain.RepositoryFactories;
using Domain.UnitOfWork;
using TewCloud.App_Start;

namespace TewCloud
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
