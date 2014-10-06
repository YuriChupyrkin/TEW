using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ConsoleUI.Autofac;
using Domain.RepositoryFactories;
using EntityFrameworkDAL.RepositoryFactories;
using Newtonsoft.Json;

namespace ConsoleUI
{
  internal class Program
  {
    public static IContainer Container = AutofacModule.RegisterAutoFac();
    public static IRepositoryFactory RepositoryFactory;

    private static void Main(string[] args)
    {
      var context = new EFRepositoryFactory();
      var user = context.UserRepository.All().First(r => r.Email == "yu4e4ko@yandex.ru");
      if (user == null)
      {
        Console.WriteLine("User is null");
      }

      var userWords = context.EnRuWordsRepository
        .AllEnRuWords().Where(r => r.UserId == user.Id);

      var words = new List<WordModel>();

      foreach (var userWord in userWords)
      {
        var word = new WordModel
        {
          EnWord = userWord.EnglishWord.EnWord,
          RuWord = userWord.RussianWord.RuWord,
          Example = userWord.Example,
          Level = userWord.WordLevel
        };

        words.Add(word);
      }

      var json = JsonConvert.SerializeObject(words);
      Console.WriteLine(json);

      var wordsFromJson = JsonConvert.DeserializeObject<IEnumerable<WordModel>>(json);

      foreach (var wordModel in wordsFromJson)
      {
        Console.WriteLine(wordModel.EnWord);
      }
    }
  }

  public class WordModel
  {
    public string EnWord { get; set; }
    public string RuWord { get; set; }
    public string Example { get; set; }
    public int Level { get; set; }
  }
}