using System.Collections.Generic;
using System.Linq;
using Domain.RepositoryFactories;
using EnglishLearnBLL.Models;

namespace EnglishLearnBLL.View
{
  public class WordViewer
  {
    private readonly IRepositoryFactory _repositoryFactory;

    public WordViewer(IRepositoryFactory repositoryFactory)
    {
      _repositoryFactory = repositoryFactory;
    }

    public IEnumerable<EnRuWordViewModel> ViewWords(int userId)
    {
      var viewList = new List<EnRuWordViewModel>();

      var enRuWords = _repositoryFactory.EnRuWordsRepository
        .AllEnRuWords().Where(r => r.UserId == userId).ToList();

      enRuWords.ForEach(r => viewList.Add(new EnRuWordViewModel
      {
        EnWord = r.EnglishWord.EnWord,
        RuWord = r.RussianWord.RuWord,
        WordLevel = r.WordLevel
      }));

      return viewList;
    } 
  }
}
