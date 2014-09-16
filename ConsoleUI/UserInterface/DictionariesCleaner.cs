using System;
using ConsoleUI.App;
using ConsoleUI.Helpers;

namespace ConsoleUI.UserInterface
{
  internal class DictionariesCleaner
  {
    public void Clean()
    {
      if (!ApplicationValidator.IsAuthorizedUser())
      {
        ConsoleWriteHelper.AccessDenied();
      }

      ConsoleWriteHelper.Clear();
      ConsoleWriteHelper.WriteHeader("Cleaning...");

      ConsoleWriteHelper.WriteLine("Wait pls...");

      var repoFactory = ApplicationContext.RepositoryFactory;

      var wasDeleted = repoFactory.EnRuWordsRepository.ClearEnWords();
      wasDeleted += repoFactory.EnRuWordsRepository.ClearRuWords();

      ConsoleWriteHelper.WriteLine("Cleaned! Deleted " + wasDeleted + " words. Press any key");
      Console.ReadLine();
      ConsoleWriteHelper.StartMenu();
    }
  }
}
