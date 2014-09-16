using System;
using ConsoleUI.App;
using ConsoleUI.UserInterface.Menu;

namespace ConsoleUI.Helpers
{
  internal static class ConsoleWriteHelper
  {
    private const string StartSymbols = "System";
    private const string HeaderSymbol = "*";
    private const string WarningSymbols = "!!!";
    private readonly static ConsoleColor DefaulColor = Console.ForegroundColor;
    private const ConsoleColor RedColor = ConsoleColor.Red;
    private const int ConsoleLen = 80;

    public static void WriteLine(string message)
    {
      Console.ForegroundColor = RedColor;
      Console.Write(StartSymbols + ": ");
      Console.ForegroundColor = DefaulColor;
      Console.WriteLine(message);
      Console.WriteLine();
    }

    public static void WriteOnNewLine(string message)
    {
      Console.ForegroundColor = RedColor;
      Console.WriteLine(StartSymbols + ":");
      Console.ForegroundColor = DefaulColor;
      Console.WriteLine(message);
      Console.WriteLine();
    }

    public static string ReadLine()
    {
      Console.Write("> ");
      var result = Console.ReadLine();
      if (CheckInputMessage(result))
      {
        return string.Empty;
      }
      Console.WriteLine();
      return result;
    }

    public static void WriteHeader(string message)
    {
      var headerSymbolCount = (ConsoleLen - (message.Length + 4))/2;

      for (int i = 0; i < headerSymbolCount; i++)
      {
        Console.Write(HeaderSymbol);
      }
      Console.Write("  {0}  ", message);

      for (int i = 0; i < headerSymbolCount; i++)
      {
        Console.Write(HeaderSymbol);
      }
      Console.WriteLine();
      Console.WriteLine();
    }

    public static void WarningMessage(string message)
    {
      Clear();
      Console.ForegroundColor = RedColor;
      Console.WriteLine("{0} {1} {2}", WarningSymbols, message.ToUpper(), WarningSymbols);
      Console.ForegroundColor = DefaulColor;
      Console.WriteLine();
      BackToStartMenu();
    }

    public static void RedMessage(string message)
    {
      Console.ForegroundColor = RedColor;
      Console.Write(StartSymbols + ": ");
      Console.WriteLine(message);
      Console.WriteLine();
      Console.ForegroundColor = DefaulColor;
    }

    public static string ReadPassword()
    {
      var pwd = string.Empty;
      Console.Write("> ");
      while (true)
      {
        ConsoleKeyInfo i = Console.ReadKey(true);
        if (i.Key == ConsoleKey.Enter)
        {
          break;
        }
        if (i.Key == ConsoleKey.Backspace)
        {
          if (pwd.Length > 0)
          {
            pwd = pwd.Substring(0, (pwd.Length - 1));
            Console.Write("\b \b");
          }
        }
        else
        {
          pwd += i.KeyChar;
          Console.Write("*");
        }
      }
      Console.WriteLine();
      Console.WriteLine();
      return pwd;
    }

    public static string ReadRusString(int minLen = -1, int maxLen = 1000)
    {
      var result = string.Empty;
      Console.Write("> ");
      while (true)
      {
        ConsoleKeyInfo i = Console.ReadKey(true);
        if (i.Key == ConsoleKey.Enter)
        {
          break;
        }
        if (i.Key == ConsoleKey.Backspace)
        {
          if (result.Length > 0)
          {
            result = result.Substring(0, (result.Length - 1));
            Console.Write("\b \b");
          }
        }
        else
        {
          var rusSymbol = EngSymbolsAutoChanger.GetRusSymbol(i.KeyChar.ToString());
          result += rusSymbol;
          Console.Write(rusSymbol);
        }
      }
      result = ValidateRusString(result, minLen, maxLen);   
      return result;
    }

    private static string ValidateRusString(string word, int minLen, int maxLen)
    {
      var message = string.Format("Max len = {0}, min len = {1}", maxLen, minLen);
      if (word.Length < minLen || word.Length > maxLen)
      {
        Console.WriteLine();
        RedMessage(message);
        word = ReadRusString(minLen, maxLen);
      }
      Console.WriteLine();
      return word;
    }

    public static void Clear()
    {
      Console.Clear();

      var userName = "Guest";
      if (ApplicationContext.CurrentUser != null)
      {
        userName = ApplicationContext.CurrentUser.Email;
      }
      Console.WriteLine(" Hi {0} ", userName);
      for (int i = 0; i < ConsoleLen; i++)
      {
        Console.Write("_");
      }
      Console.WriteLine();
    }

    public static void IsExit()
    {
      Clear();
      WriteLine("Exit? (y/n)");

      var answer = ReadLine();
      if (answer.Equals("y", StringComparison.OrdinalIgnoreCase))
      {
        Environment.Exit(1);
      }
      StartMenu();
    }

    public static void StartMenu()
    {
      new StartMenu().WriteMenu();
    }

    public static void IncorrectLoginOrPassword()
    {
      WarningMessage("Incorrect login or password");
      BackToStartMenu();
    }

    public static void BackToStartMenu()
    {
      WriteLine("Back to start menu? (y/n)");
      var answer = ReadLine();
      if (answer.Equals("y", StringComparison.OrdinalIgnoreCase))
      {
        StartMenu();
      }
      else
      {
        IsExit();
      }
    }

    public static void AccessDenied()
    {
      WarningMessage("Access denied!");
    }

    public static string ReadLineWithValidationLen(int minLen, int maxLen)
    {
      var word = ReadLine();
      var message = string.Format("Max len = {0}, min len = {1}", maxLen, minLen);
      if (word.Length < minLen || word.Length > maxLen)
      {
        var isWrited = false;
        while (!isWrited)
        {
          RedMessage(message);
          word = ReadLine();
          if (word.Length >= minLen && word.Length < maxLen)
          {
            isWrited = true;
          }
        }
      }
      return word;
    }

    private static bool CheckInputMessage(string message)
    {
      message = message.ToLower();
      var isSpecMessage = true;

      switch (message)
      {
        case "!menu":
          StartMenu();
          break;
        case "!exit":
          Environment.Exit(1);
          break;
        default:
          isSpecMessage = false;
          break;
      }

      return isSpecMessage;
    }
  }
}
