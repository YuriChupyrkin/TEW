using System;
using System.Text.RegularExpressions;

namespace WpfUI.Helpers
{
  internal static class ApplicationValidator
  {
    public static bool IsValidatEmail(string email)
    {
      if (string.IsNullOrEmpty(email))
      {
        return false;
      }
      return Regex.IsMatch(email,
        @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"); 
    }

    public static bool IsValidatPassword(string password)
    {
      const int minLength = 4;
      const int maxLength = 16;
      return password.Length >= minLength && password.Length <= maxLength;
    }

    public static bool IsPasswordEquals(string password, string confirmPassword)
    {
      return password == confirmPassword;
    }

    public static bool IsAuthorizedUser()
    {
      return ApplicationContext.CurrentUser != null;
    }

    public static void ExpectAuthorized()
    {
      if (!IsAuthorizedUser())
      {
        throw new Exception("Access denied");
      }
    }
  }
}
