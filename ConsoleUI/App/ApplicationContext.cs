using System;
using System.Collections.Generic;
using Autofac;
using Domain.Entities;
using Domain.RepositoryFactories;

namespace ConsoleUI.App
{
  internal static class ApplicationContext
  {
    public static User CurrentUser { get; set; }
    public static IContainer AutoFacContainer { get; set; }
    public static IRepositoryFactory RepositoryFactory { get; set; }
  }
}
