using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleUI.App;
using ConsoleUI.Helpers;
using Domain.RepositoryFactories;

namespace ConsoleUI.UserInterface
{
  internal class AddWord
  {
    private readonly IRepositoryFactory _repositoryFactory;

    public AddWord()
    {
      _repositoryFactory = ApplicationContext.RepositoryFactory;
    }
    public void Add()
    {
      if (!ApplicationValidator.IsAuthorizedUser())
      {
        ConsoleWriteHelper.AccessDenied();
      }

      ConsoleWriteHelper.Clear();
      ConsoleWriteHelper.WriteHeader("Add word");
      ConsoleWriteHelper.WriteLine("Write word(s)");

      var word = ConsoleWriteHelper.ReadLineWithValidationLen(2, 150);

      var rusWords = _repositoryFactory.EnRuWordsRepository.GetTranslate(word);

      var enumerable = rusWords as IList<string> ?? rusWords.ToList();
      CreateTranslateList(enumerable);

      string peeked = ConsoleWriteHelper.ReadLine();
     
      try
      {
        var wordNo = 0;
        wordNo = int.Parse(peeked);
        if (wordNo > enumerable.Count())
        {
          ConsoleWriteHelper.WarningMessage("Incorrect answer");
        }

        var rusWord = string.Empty;

        if (wordNo != 0)
        {
          rusWord = enumerable.ToList()[wordNo - 1];
        }
        else
        {
          ConsoleWriteHelper.WriteLine("Enter your value:");
          rusWord = ConsoleWriteHelper.ReadRusString(2, 150);
          if (string.IsNullOrWhiteSpace(rusWord))
          {
            _repositoryFactory
              .EnRuWordsRepository
              .DeleteEnRuWord(word, ApplicationContext.CurrentUser.Id);
            ConsoleWriteHelper.RedMessage("\"" + word + "\" deleted");
            Console.ReadLine();
            ConsoleWriteHelper.StartMenu();
          }
        }

        AddToDataBase(word, rusWord);

        ConsoleWriteHelper.WriteLine("Added... Press any key");
        Console.ReadLine();
        ConsoleWriteHelper.StartMenu();
      }
      catch
      {
        ConsoleWriteHelper.WarningMessage("Enter number!!!");
      }

    }

    private void AddToDataBase(string enWord, string ruWord)
    {
      try
      {
        var userId = ApplicationContext.CurrentUser.Id;
        _repositoryFactory.EnRuWordsRepository.AddTranslate(enWord, ruWord, userId);
      }
      catch (Exception ex)
      {
        ConsoleWriteHelper.WarningMessage(ex.Message);
      }
    }

    private void CreateTranslateList(IEnumerable<string> rusWords)
    {
      var count = 1;
      var message = "Choose: \n";

      message += "0 - My translate...\n";

      foreach (var rusWord in rusWords)
      {
        message += string.Format("{0} - {1};\n", count, rusWord);
        count++;
      }
      ConsoleWriteHelper.WriteOnNewLine(message);
    }
  }
}
