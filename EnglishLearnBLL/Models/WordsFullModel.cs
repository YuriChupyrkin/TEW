using Domain.RepositoryFactories.Models;
using System.Collections.Generic;

namespace EnglishLearnBLL.Models
{
  public sealed class WordsFullModel
  {
    public WordsFullModel()
    {
      Words = new List<SimpleWordModel>();
    }

    public string UserName { get; set; }

    public int UserId { get; set; }

    public int TotalWords { get; set; }

    public IList<SimpleWordModel> Words { get; set; }
  }
}