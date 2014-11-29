using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Autofac;
using ConsoleUI.Autofac;
using Domain.RepositoryFactories;
using EnglishLearnBLL.Models;
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
      var число = 5;
      Console.WriteLine(число);
      //var list = new List<int>();
      //for (var i = 0; i < 100; i++)
      //{
      //  list.Add(i);
      //}

      //var iterationCount = 0;
      //if (list.Count%10 != 0)
      //{
      //  iterationCount = list.Count/10 + 1;
      //}
      //else
      //{
      //  iterationCount = list.Count / 10;
      //}

      //var res = list.Take(10);
      //Console.WriteLine("0___");
      //PrintList(res);
      //for (var i = 1; i < iterationCount; i++)
      //{
      //  res = list.Skip(10 * i).Take(10);
      //  Console.WriteLine(i + "___");
      //  PrintList(res);
      //}
    }

    private static void PrintList(IEnumerable<int> list)
    {
      foreach (var i in list)
      {
        Console.Write("{0}; ", i);
      }
      Console.WriteLine();
    }

    static void KickServer()
    {
      var uri = "http://localhost:8081/api/Values";
      var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);

      string[] result = null;
      try
      {
        WebResponse response = httpWebRequest.GetResponse();
        var responseStream = response.GetResponseStream();
        using (var stream = new StreamReader(responseStream))
        {
          var responseString = stream.ReadToEnd();
          result = JsonConvert.DeserializeObject<string[]>(responseString);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }

      foreach (var s in result)
      {
        Console.WriteLine(s);
      }
    }

    static void Get()
    {
      var uri = "http://localhost:8081/api/Synchronize" + "?userName=yu4e4ko@yandex.ru";
      var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);

      WordsCloudModel result = null;

      try
      {
        WebResponse response = httpWebRequest.GetResponse();
        var responseStream = response.GetResponseStream();
        using (var stream = new StreamReader(responseStream))
        {
          var responseString = stream.ReadToEnd();
          result = JsonConvert.DeserializeObject<WordsCloudModel>(responseString);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }

      foreach (var word in result.Words)
      {
        Console.WriteLine(word.English);
      }
    }



    private static void SendObj()
    {
      var context = new EFRepositoryFactory();
      var user = context.UserRepository.All().First(r => r.Email == "yu4e4ko@yandex.ru");
      if (user == null)
      {
        Console.WriteLine("User is null");
      }

      var userWords = context.EnRuWordsRepository
        .AllEnRuWords().Where(r => r.UserId == user.Id);

      var words = new List<WordJsonModel>();

      foreach (var userWord in userWords)
      {
        var word = new WordJsonModel
        {
          English = userWord.EnglishWord.EnWord,
          Russian = userWord.RussianWord.RuWord,
          Example = userWord.Example,
          Level = userWord.WordLevel
        };

        words.Add(word);
      }

      var cloudModel = new WordsCloudModel
      {
        UserName = user.Email,
        Words = words
      };

      var json = JsonConvert.SerializeObject(cloudModel);
      Console.WriteLine(json);


      //request 
      HttpWebRequest req = (HttpWebRequest)WebRequest
        .Create("http://localhost:8081/api/Synchronize");

      UTF8Encoding enc = new UTF8Encoding();
      string stringData = json; //place body here

      byte[] data = enc.GetBytes(stringData);

      req.Method = "POST";
      req.ContentType = "application/json; charset=utf-8"; //place MIME type here
      req.ContentLength = data.Length;

      Stream newStream = req.GetRequestStream();
      newStream.Write(data, 0, data.Length);
      newStream.Close();

      var response = req.GetResponse();

      Stream receiveStream = response.GetResponseStream();
      ResponseModel result;

      using (var streamReader = new StreamReader(receiveStream, Encoding.UTF8))
      {
        var jsonResult = streamReader.ReadToEnd();
        result = JsonConvert.DeserializeObject<ResponseModel>(jsonResult);
      }

      if (receiveStream != null)
      {
        receiveStream.Close();
      }

      Console.WriteLine("result: {0}", result.IsError);
    }
  }
}