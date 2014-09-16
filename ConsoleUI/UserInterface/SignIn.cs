using System;
using ConsoleUI.App;
using ConsoleUI.Authorize;
using ConsoleUI.Helpers;
using Domain.Entities;

namespace ConsoleUI.UserInterface
{
  internal class SignIn
  {
    public User SignInUser()
    {
      ConsoleWriteHelper.Clear();
      var login = string.Empty;
      var password = string.Empty;

      ConsoleWriteHelper.WriteHeader("Sign in");
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

      var userProvider = new UserProvider(ApplicationContext.RepositoryFactory);

      User user = null;
      try
      {
        user = userProvider.ValidateUser(login, password);
      }
      catch (Exception ex)
      {
        ConsoleWriteHelper.WarningMessage(ex.Message);
      }

      if (user != null)
      {
        ApplicationContext.CurrentUser = user;
        ConsoleWriteHelper.StartMenu();
      }
      ConsoleWriteHelper.IncorrectLoginOrPassword();
      return user;
    }
  }
}
