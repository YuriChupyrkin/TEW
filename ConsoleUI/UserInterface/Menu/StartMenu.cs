using System;
using System.Text;
using ConsoleUI.App;
using ConsoleUI.Helpers;

namespace ConsoleUI.UserInterface.Menu
{
  internal class StartMenu : IMenu
  {
    private const string SignUpKey = "2";
    private const string SignInKey = "1";
    private const string AddWordKey = "3";
    private const string MyWordsKey = "4";
    private const string EnRuTestKey = "5";
    private const string RuEnTestKey = "6";
    private const string WriteTestKey = "7";
    private const string CleanKey = "9";
    private const string ExitKey = "0";

    public void WriteMenu()
    {
      ConsoleWriteHelper.Clear();
      ConsoleWriteHelper.WriteHeader("Start menu");

      var stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat("{0} -> Sign In", SignInKey).AppendLine();
      stringBuilder.AppendFormat("{0} -> Sign Up", SignUpKey).AppendLine();

      if (ApplicationContext.CurrentUser != null)
      {
        stringBuilder.AppendFormat("{0} -> Add Word", AddWordKey).AppendLine();
        stringBuilder.AppendFormat("{0} -> My Words", MyWordsKey).AppendLine();
        stringBuilder.AppendFormat("{0} -> English-Russian Test", EnRuTestKey).AppendLine();
        stringBuilder.AppendFormat("{0} -> Russian-English Test", RuEnTestKey).AppendLine();
        stringBuilder.AppendFormat("{0} -> Write Test", WriteTestKey).AppendLine();
        stringBuilder.AppendFormat("{0} -> Clean dictionaries", CleanKey).AppendLine();
      }

      stringBuilder.AppendFormat("{0} -> Exit", ExitKey).AppendLine();

      var menuText = stringBuilder.ToString();

      ConsoleWriteHelper.WriteOnNewLine(menuText);

      var key = ConsoleWriteHelper.ReadLine();
      MenuChoose(key);    
    }

    private static void MenuChoose(string key)
    {
      var pickerTest = new PickerTest();
      switch (key)
      {
        case SignUpKey:
          var signUp = new SignUp();
          signUp.SignUpUser();
          break;
        case SignInKey:
          var signIn = new SignIn();
          signIn.SignInUser();
          break;
        case AddWordKey:
          var addWord = new AddWord();
          addWord.Add();
          break;
        case MyWordsKey:
          var myWordList = new MyWordList();
          myWordList.GetList();
          break;
        case EnRuTestKey:
          pickerTest.StartEngRusTest();
          break;
        case RuEnTestKey:
          pickerTest.StartRusEngTest();
          break;
        case WriteTestKey:
          var writeTest = new WriteTest();
          writeTest.StartTest();
          break;
        case CleanKey:
          var cleaner = new DictionariesCleaner();
          cleaner.Clean();
          break;
        case ExitKey:
          Environment.Exit(1);
          break;
        default:
          ConsoleWriteHelper.Clear();
          ConsoleWriteHelper.IsExit();
          break;
      }
    }
  }
}
