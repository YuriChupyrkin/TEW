using Domain.RepositoryFactories.Models;
using EnglishLearnBLL.Models;
using System.Collections.Generic;

namespace WebSite.Models
{
  public class UserStatModel
  {
    public long WordsLevel { get; set; }

    public int WordsCount { get; set; }

    public string Email { get; set; }

    public string NickName { get; set; }

    public int Id { get; set; }

    public string UniqueId { get; set; }

    public IList<SimpleWordModel> MostFailedWords { get; set; }

    public IList<SimpleWordModel> MostStudiedWords { get; set; }
  }
}