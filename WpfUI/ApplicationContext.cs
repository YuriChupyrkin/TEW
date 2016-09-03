using Autofac;
using Common.Mail;
using Domain.Entities;
using Domain.RepositoryFactories;

namespace WpfUI
{
  internal static class ApplicationContext
  {
    public static User CurrentUser { get; set; }
    public static IContainer AutoFacContainer { get; set; }
    public static IEmailSender EmailSender { get; set; }
  }
}
