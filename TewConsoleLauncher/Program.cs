using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Newtonsoft.Json;
using TewLauncher.Models;

namespace TewConsoleLauncher
{
  class Program
  {
    //private const string Uri = "http://localhost:8081/";
    private const string Uri = "http://yu4e4ko.somee.com/TewCloud/";

    private const string ClientFilesInfoController = "api/ClientFilesInfo";
    private const string ClientFilesUpdateController = "api/ClientFilesUpdate";
    private static string _clientPath;
    private static List<FilePathWithUpdateTime> _updateFiles; 

    static void Main(string[] args)
    {
      try
      {
        StartLauncher();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }

    private static void StartLauncher()
    {
      Console.ForegroundColor = ConsoleColor.Green;
      var currentDir = Directory.GetCurrentDirectory();
      _clientPath = currentDir + "/client";
      _updateFiles = new List<FilePathWithUpdateTime>();

      if (CheckConnection())
      {
        var filesInfoFromServer = GetFilesInfo();
        CheckUpdateFiles(filesInfoFromServer);

        if (_updateFiles.Count > 0)
        {
          ShowUpdateInfo();

          if (ShowYesNoDialog("\nstart update(Y/N)"))
          {
            StartUpdate();
            Console.WriteLine("Complete");
            StartClientAndCloseLauncher();
          }
          else
          {
            Environment.Exit(1);
          }
        }
        else
        {
          StartClientAndCloseLauncher();
        }
      }
      else
      {
        if (ShowYesNoDialog("Connection error! Start old version? (Y/N)")) ;
        {
          StartClientAndCloseLauncher();
        }
      }   
    }

    private static void StartClientAndCloseLauncher()
    {
      System.Diagnostics.Process.Start(_clientPath + "/WpfUI.exe");
      Thread.Sleep(1000);
      Environment.Exit(1);
    }

    private static void ShowUpdateInfo()
    {
      var filesCoint = _updateFiles.Count;
      var size = _updateFiles.Sum(r => r.FileSize);

      var totalSizeKb = size/1024;

      Console.WriteLine("Update: {0} file(s)\nTotal size: {1} Kb", filesCoint, totalSizeKb);
    }

    private static bool ShowYesNoDialog(string message)
    {
      Console.WriteLine(message);
      var key = Console.ReadKey();
      Console.WriteLine();
      return key.Key == ConsoleKey.Y;
    }

    private static void CheckUpdateFiles(FilesInfo filesInfo)
    {
      foreach (var filePathWithUpdateTime in filesInfo.FilePathWithUpdateTimes)
      {
        if (filePathWithUpdateTime.FilePath.Contains(".sdf"))
        {
          continue;
        }

        var fullFilePath = _clientPath + filePathWithUpdateTime.FilePath;

        var isExist = new FileInfo(fullFilePath).Exists;
        if (isExist)
        {
          if (filePathWithUpdateTime.UpdateTime > File.GetLastWriteTime(fullFilePath))
          {
            _updateFiles.Add(filePathWithUpdateTime);
          }
        }
        else
        {
          _updateFiles.Add(filePathWithUpdateTime);
        }
      }
    }

    private static void StartUpdate()
    {
      Console.Clear();
      Console.WriteLine("Updating... please wait...");

      var count = 0;
      var totalCount = _updateFiles.Count;

      foreach (var filePathWithUpdateTime in _updateFiles)
      {
        CheckDirs(filePathWithUpdateTime.FilePath);
        UpdateFile(filePathWithUpdateTime.FilePath);
        count++;

        Console.WriteLine("updated {0} from {1}", count, totalCount);
      }
    }

    private static void CheckDirs(string filePath)
    {
      var dirs = filePath.Split('\\');
   
      if (dirs.Length > 1)
      {
        var directoryPath = _clientPath;
        for (var i = 0; i < dirs.Length - 1; i++)
        {
          directoryPath += "/" + dirs[i];
          if (Directory.Exists(directoryPath) == false)
          {
            Directory.CreateDirectory(directoryPath);
          }
        }
      }
    }

    private static FilesInfo GetFilesInfo()
    {
      var uri = Uri + ClientFilesInfoController;
  
      try
      {
        var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
        WebResponse response = httpWebRequest.GetResponse();
        var responseStream = response.GetResponseStream();

        using (var stream = new StreamReader(responseStream))
        {
          var responseString = stream.ReadToEnd();
          var result = JsonConvert.DeserializeObject<FilesInfo>(responseString);

          return result;
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Error! " + ex.Message, ex);
      }
    }

    public static void UpdateFile(string fileName)
    {
      try
      {
        var uri = Uri + ClientFilesUpdateController + "?fileName=" + fileName;
        var filePath = _clientPath + "/" + fileName;

        byte[] result = null;
        var buffer = new byte[4097];

        var wr = WebRequest.Create(uri);

        var response = wr.GetResponse();
        var responseStream = response.GetResponseStream();
        var memoryStream = new MemoryStream();

        int count = 0;

        do
        {
          count = responseStream.Read(buffer, 0, buffer.Length);
          memoryStream.Write(buffer, 0, count);

          if (count == 0)
          {
            break;
          }
        } while (true);

        result = memoryStream.ToArray();

        var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

        fs.Write(result, 0, result.Length);

        fs.Close();
        memoryStream.Close();
        responseStream.Close();
      }
      catch (Exception ex)
      {
        throw new Exception("Error! " + ex.Message, ex);
      }
    }

    private static bool CheckConnection()
    {
      var client = new WebClient();
      try
      {
        using (client.OpenRead(Uri))
        {
        }
        return true;
      }
      catch (WebException)
      {
        return false;
      }
    }
  }
}
