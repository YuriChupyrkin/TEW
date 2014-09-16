using System;
using System.Net.Mail;
using ConsoleUI.App;

namespace ConsoleUI.Helpers
{
  internal static class ApplicationValidator
  {
    public static bool IsValidatEmail(string email)
    {
      if (string.IsNullOrEmpty(email))
      {
        return false;
      }
      try
      {
        var mail = new MailAddress(email);
        return true;
      }
      catch (FormatException)
      {
        return false;
      }
    }

    public static bool IsValidatPassword(string password)
    {
      const int minLength = 4;
      const int maxLength = 16;
      return password.Length > minLength && password.Length < maxLength;
    }

    public static bool IsPasswordEquals(string password, string confirmPassword)
    {
      return password == confirmPassword;
    }

    public static bool IsAuthorizedUser()
    {
      return ApplicationContext.CurrentUser != null;
    }
  }
}
