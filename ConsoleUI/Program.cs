using System;
using System.Collections.Generic;
using System.IO;
using Autofac;
using ConsoleUI.Autofac;
using Domain.RepositoryFactories;
using TewLauncher.Models;
using TewLauncher.Services;

namespace ConsoleUI
{
  internal class Program
  {
    public static IContainer Container = AutofacModule.RegisterAutoFac();
    public static IRepositoryFactory RepositoryFactory;

    private static void Main(string[] args)
    {
      var a = 3;
      var b = 5;
      var c = (double)b / a;

      Console.WriteLine(c);

      /*
      var dir = Directory.GetCurrentDirectory();

      var result = new FileInfoChecker(@"D://DCDownloads/Debug").GetFilesInfo();

      //foreach (var filePathWithUpdateTime in result.FilePathWithUpdateTimes)
      //{
      //  Console.WriteLine("{0}; {1}", filePathWithUpdateTime.FilePath, filePathWithUpdateTime.UpdateTime);
      //  Console.WriteLine("---------------------------------------------------------");
      //}
      CompareFiles(result);
      */
    }

    private static void PrintList<T>(IEnumerable<T> list)
    {
      Console.WriteLine("start list print");
      foreach (var i in list)
      {
        Console.WriteLine(i);
      }
      Console.WriteLine("end list print");
    }


    private static void CompareFiles(FilesInfo result)
    {
      foreach (var filePathWithUpdateTime in result.FilePathWithUpdateTimes)
      {
        var currentDir = Directory.GetCurrentDirectory();

        var fullFilePath = currentDir + filePathWithUpdateTime.FilePath;

        var isExist = new FileInfo(fullFilePath).Exists;
        if (isExist)
        {
          if (filePathWithUpdateTime.UpdateTime > File.GetLastWriteTime(fullFilePath))
          {
            Console.WriteLine("-----------UPDATE FILE: {0}", filePathWithUpdateTime.FilePath);
          }
          else
          {
            Console.WriteLine("NOT UPDATE FILE: {0}", filePathWithUpdateTime.FilePath);
          }
        }
        else
        {
          Console.WriteLine("-------------ADD FILE: {0}", filePathWithUpdateTime.FilePath);
        }
      }
    }
  }
}