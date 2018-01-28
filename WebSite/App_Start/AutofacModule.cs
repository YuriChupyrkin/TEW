using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Common.Mail;
using Domain.RepositoryFactories;
using EntityFrameworkDAL.RepositoryFactories;

namespace WebSite
{
  public sealed class AutofacModule
  {
    public static void RegisterAutoFac()
    {
      var builder = new ContainerBuilder();
      builder.RegisterControllers(Assembly.GetExecutingAssembly());
      builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

      builder.RegisterType<EFRepositoryFactory>().As<IRepositoryFactory>()
          .InstancePerRequest();

      builder.RegisterType<YandexMailSender>().As<IEmailSender>()
          .InstancePerRequest();

      var container = builder.Build();
      DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

      var config = GlobalConfiguration.Configuration;
      var resolver = new AutofacWebApiDependencyResolver(container);
      config.DependencyResolver = resolver;
    }
  }
}