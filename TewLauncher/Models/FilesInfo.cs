using System.Collections.Generic;

namespace TewLauncher.Models
{
  public class FilesInfo
  {
    public FilesInfo()
    {
      FilePathWithUpdateTimes = new List<FilePathWithUpdateTime>();
    }

    public IList<FilePathWithUpdateTime> FilePathWithUpdateTimes { get; private set; }
  }
}
