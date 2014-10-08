using System.Collections.Generic;

namespace EnglishLearnBLL.Models
{
  public sealed class WordsCloudModel
  {
    public WordsCloudModel()
    {
      Words = new List<EnRuWordViewModel>();
    }

    public string UserName { get; set; }
    public IList<EnRuWordViewModel> Words { get; set; } 
  }
}
