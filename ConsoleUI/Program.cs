using System;
using System.IO;
using System.Net;
using Autofac;
using ConsoleUI.Autofac;
using ConsoleUI.Helpers;
using Domain.RepositoryFactories;
using ApplicationContext = ConsoleUI.App.ApplicationContext;

namespace ConsoleUI
{
  internal class Program
  {
    public static IContainer Container = AutofacModule.RegisterAutoFac();
    public static IRepositoryFactory RepositoryFactory;

    private static void Main(string[] args)
    {
      var testStr = " test str ";
      var tmp = testStr.Replace(" ", "*");
      Console.WriteLine(tmp);

      testStr = testStr.Trim();
      tmp = testStr.Replace(" ", "*");
      Console.WriteLine(tmp);

      //Test();
      //////////// Start ///////////////////
      //Console.WindowWidth = 80;
      //Console.WindowHeight = 40;
      //Console.ForegroundColor = ConsoleColor.Green;

      //RepositoryFactory = Container.BeginLifetimeScope().BeginLifetimeScope().Resolve<IRepositoryFactory>();
      //ApplicationContext.AutoFacContainer = Container;
      //ApplicationContext.RepositoryFactory = RepositoryFactory;
      //ConsoleWriteHelper.Clear();

      //StartProgramm();

      //ConsoleWriteHelper.WarningMessage("Something wrong...");
      //Console.ReadLine();
    }

    private static void Test()
    {
      var word = "witcher";
      word = word.Replace(" ", "%20");
      string uri =
      "https://translate.google.by/translate_a/single?client=t&sl=en&tl=ru&hl=ru&dt=bd&dt=ex&dt=ld&dt=md&dt=qc&dt=rw&dt=rm&dt=ss&dt=t&dt=at&dt=sw&ie=UTF-8&oe=UTF-8&otf=2&ssel=0&tsel=0&q=" + word;

      var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
      //httpWebRequest.Headers.Add("Authorization", authToken);

      try
      {
        var response = httpWebRequest.GetResponse();
        using (StreamReader stream = new StreamReader(response.GetResponseStream()))
        {
          var result = stream.ReadToEnd();
          ParseGoogleResponse(result);
          GetContext(result);
        }
      }
      catch(Exception ex)
      {
        //swallow exceptions
        Console.WriteLine(ex.Message);
      }

    }

    private static string ParseGoogleResponse(string response)
    {
      var tmpData = string.Empty;

      var bracketsCount = 0;
      for (var i = 0; i < response.Length; i++)
      {
        if (response[i] == '[')
        {
          bracketsCount++;
          if (bracketsCount == 7)
          {
            tmpData = response.Substring(i + 1);
            if (tmpData[0] != '\"')
            {
              tmpData = GetFirst(response);
            }
            break;
          }
        }
      }

      for (var i = 0; i < tmpData.Length; i++)
      {
        if (tmpData[i] == ']')
        {
          tmpData = tmpData.Substring(0, i);
          break;
        }
      }

      tmpData = tmpData.Replace("\"", "");

      var resultArray = tmpData.Split(',');

      foreach (var s in resultArray)
      {
        Console.WriteLine(s);
      }

      return string.Empty;
    }

    private static string GetFirst(string response)
    {
      var tmpData = string.Empty;

      var index = response.IndexOf("\"");

      if (index == -1)
      {
        return tmpData;
      }

      tmpData = response.Substring(index + 1);
      index = tmpData.IndexOf("\"");

      if (index == -1)
      {
        return string.Empty;
      }

      tmpData = tmpData.Substring(0, index);
      return tmpData;
    }

    private static void GetContext(string response)
    {
      var tmpData = string.Empty;
      var bracketsCount = 0;

      var index = response.IndexOf(".001\",\"");
      if (index != -1)
      {
        tmpData = response.Substring(index + 7);

        index = tmpData.IndexOf("\"");

        if (index != -1)
        {
          tmpData = tmpData.Substring(0, index);
        }
        
      }
      Console.WriteLine(tmpData);
    }

    #region privates
    private static void StartProgramm()
    {
      try
      {
        ConsoleWriteHelper.StartMenu();
      }
      catch (Exception ex)
      {
        ConsoleWriteHelper.RedMessage(ex.Message);
        Console.ReadLine();
        StartProgramm();
      }
    }

    #endregion
  }
}