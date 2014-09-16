using Autofac;
using Common.Mail;
using Domain.RepositoryFactories;
using EntityFrameworkDAL.RepositoryFactories;

namespace WpfUI.Autofac
{
  internal class AutofacModule
  {
    public static IContainer RegisterAutoFac()
    {
      var builder = new ContainerBuilder();

      builder.RegisterType<EFRepositoryFactory>().As<IRepositoryFactory>()
        .InstancePerLifetimeScope();

      builder.RegisterType<YandexMailSender>().As<IEmailSender>()
        .InstancePerLifetimeScope();

      IContainer container = builder.Build();
      return container;
    }
  }
}
