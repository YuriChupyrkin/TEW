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

    public IEnumerable<WordViewModel> ViewWords(int userId)
    {
      var viewList = new List<WordViewModel>();

      var enRuWords = _repositoryFactory.EnRuWordsRepository
        .AllEnRuWords().Where(r => r.UserId == userId && r.IsDeleted == false).ToList();

      enRuWords.ForEach(r => viewList.Add(new WordViewModel
      {
        English = r.EnglishWord.EnWord,
        Russian = r.RussianWord.RuWord,
        Level = r.WordLevel,
        Example = r.Example
      }));

      return viewList;
    } 
  }
}
