using System;

namespace TewLauncher.Models
{
  public class FilePathWithUpdateTime
  {
    public FilePathWithUpdateTime(string filePath, DateTime updateTime, long fileSize)
    {
      FilePath = filePath;
      UpdateTime = updateTime;
      FileSize = fileSize;
    }

    public FilePathWithUpdateTime()
    {
    }

    public string FilePath { get; set; }
    public DateTime UpdateTime { get; set; }
    public long FileSize { get; set; }
  }
}
