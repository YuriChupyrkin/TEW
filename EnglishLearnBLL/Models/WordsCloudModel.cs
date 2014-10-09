using System.Collections.Generic;

namespace EnglishLearnBLL.Models
{
  public sealed class WordsCloudModel
  {
    public WordsCloudModel()
    {
      Words = new List<WordViewModel>();
    }

    public string UserName { get; set; }
    public IList<WordViewModel> Words { get; set; } 
  }
}
