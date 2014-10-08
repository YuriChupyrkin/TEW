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
      //SendObj();
      //Get();
      KickServer();
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

      var words = new List<EnRuWordViewModel>();

      foreach (var userWord in userWords)
      {
        var word = new EnRuWordViewModel
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