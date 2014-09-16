using System;
using System.Linq;
using ConsoleUI.App;
using ConsoleUI.Helpers;
using EnglishLearnBLL.View;

namespace ConsoleUI.UserInterface
{
  internal class MyWordList
  {
    public void GetList()
    {
      if (!ApplicationValidator.IsAuthorizedUser())
      {
        ConsoleWriteHelper.AccessDenied();
      }

      ConsoleWriteHelper.Clear();
      ConsoleWriteHelper.WriteHeader("My words");

      var userId = ApplicationContext.CurrentUser.Id;
      var wordViewer = new WordViewer(ApplicationContext.RepositoryFactory);

      var words = wordViewer.ViewWords(userId)
        .OrderByDescending(r => r.WordLevel);

      var resultString = words.Aggregate("My words:\n\n",
        (current, word) => current + string.Format("{0} <-> {1} ({2})\n",
          word.EnWord, word.RuWord, word.WordLevel));

      resultString += "\nTotal: " + words.Count();

      ConsoleWriteHelper.WriteOnNewLine(resultString);
      ConsoleWriteHelper.WriteLine("Press any key");
      Console.ReadLine();
      ConsoleWriteHelper.StartMenu();
    }
  }
}
