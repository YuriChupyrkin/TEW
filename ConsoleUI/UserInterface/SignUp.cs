using System;
using System.Net.Mail;
using ConsoleUI.App;
using ConsoleUI.Authorize;
using ConsoleUI.Helpers;
using Domain.Entities;
using Domain.RepositoryFactories;

namespace ConsoleUI.UserInterface
{
  internal class SignUp
  {
    public User SignUpUser()
    {
      ConsoleWriteHelper.Clear();
      var login = string.Empty;
      var password = string.Empty;
      var confirmPassword = string.Empty;

      ConsoleWriteHelper.WriteHeader("Sign up");
      ConsoleWriteHelper.WriteLine("Enter email");
      login = ConsoleWriteHelper.ReadLine();

      if (!ApplicationValidator.IsValidatEmail(login))
      {
        ConsoleWriteHelper.WarningMessage("Incorrect email");
        return null;
      }

      ConsoleWriteHelper.WriteLine("Enter password");
      password = ConsoleWriteHelper.ReadPassword();

      if (!ApplicationValidator.IsValidatPassword(password))
      {
        ConsoleWriteHelper.WarningMessage("Incorrect password! (Min len = 4, Max len = 16");
        return null;
      }

      ConsoleWriteHelper.WriteLine("Confirm password");
      confirmPassword = ConsoleWriteHelper.ReadPassword();

      if (!ApplicationValidator.IsPasswordEquals(password, confirmPassword))
      {
        ConsoleWriteHelper.WarningMessage("Password and confirm password must be equal");
        return null;
      }

      var user = CreateUser(login, password);
      if (user == null)
      {
        ConsoleWriteHelper.WarningMessage("User already exist");
        return null;
      }

      ApplicationContext.CurrentUser = user;
      ConsoleWriteHelper.StartMenu();
      return user;
    }

    private User CreateUser(string login, string password)
    {
      var userProvider = new UserProvider(ApplicationContext.RepositoryFactory);
      return userProvider.CreateUser(login, password);
    }
  }
}