using System.IO;
using TewLauncher.Models;

namespace TewLauncher.Services
{
  public class FileInfoChecker
  {
    private FilesInfo _filesInfo;
    private string _dirPath;

    public FileInfoChecker(string dirPath)
    {
      _dirPath = dirPath;
    }

    public FilesInfo GetFilesInfo()
    {
      _filesInfo = new FilesInfo();
      CheckDirectories(_dirPath);
      return _filesInfo;
    }

    private void CheckDirectories(string dir)
    {
      var directories = Directory.GetDirectories(dir);
      CheckFiles(dir);

      if (directories.Length > 0)
      {
        foreach (var directory in directories)
        {          
          CheckDirectories(directory);
        }   
      }

    }

    private void CheckFiles(string dir)
    {
      var files = Directory.GetFiles(dir);

      foreach (var file in files)
      {
        var fileUpdateTime = File.GetLastWriteTime(file);
        var filePath = file.Remove(0, _dirPath.Length);
        var fileSize = new FileInfo(file).Length;

        _filesInfo.FilePathWithUpdateTimes.Add(new FilePathWithUpdateTime(filePath, fileUpdateTime, fileSize));
      }
    }
  }
}
