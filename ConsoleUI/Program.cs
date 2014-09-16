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
      var fromLang = "en";
      var tobetranslated = "Hello";
      var toLang = "ru";
      var appId = "TrainerOfEnglishWords";

      try
      {
        string uri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?appId="
                     + appId + "&text=" + tobetranslated + "&from=" + fromLang + "&to=" + toLang;
        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(uri);
        WebResponse response = request.GetResponse();
        Stream strm = response.GetResponseStream();
        StreamReader reader = new System.IO.StreamReader(strm);
        var translatedText = reader.ReadToEnd();
        Console.WriteLine("The translated text is: '" + translatedText + "'.");
        response.Close();

      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }

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
  }
}