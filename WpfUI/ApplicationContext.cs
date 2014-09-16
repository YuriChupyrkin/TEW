using Autofac;
using Common.Mail;
using Domain.Entities;
using Domain.RepositoryFactories;
using WpfUI.Auth;
using WpfUI.Helpers;

namespace WpfUI
{
  internal static class ApplicationContext
  {
    public static User CurrentUser { get; set; }
    public static IContainer AutoFacContainer { get; set; }
    public static IRepositoryFactory RepositoryFactory { get; set; }
    public static AdminAuthentication AdminAuthentication { get; set; }
    public static BingTranslater BingTranslater { get; set; }
    public static IEmailSender EmailSender { get; set; }
  }
}
