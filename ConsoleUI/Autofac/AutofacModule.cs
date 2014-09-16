using Autofac;
using Domain.RepositoryFactories;
using EntityFrameworkDAL.RepositoryFactories;

namespace ConsoleUI.Autofac
{
  public class AutofacModule
  {
    public static IContainer RegisterAutoFac()
    {
      var builder = new ContainerBuilder();

      builder.RegisterType<EFRepositoryFactory>().As<IRepositoryFactory>()
        .InstancePerLifetimeScope();

      IContainer container = builder.Build();
      return container;
    }
  }
}