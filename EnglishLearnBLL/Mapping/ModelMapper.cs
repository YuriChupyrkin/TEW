using System.Collections.Generic;
using Domain.Entities;
using EnglishLearnBLL.Models;

namespace EnglishLearnBLL.Mapping
{
  public class ModelMapper
  {
    public static IEnumerable<WordViewModel> EnRuWordsToViewModels(
      IEnumerable<EnRuWord> enRuWords)
    {
      var models = new List<WordViewModel>();

      foreach (var enRuWord in enRuWords)
      {
        var viewModel = new WordViewModel
        {
          English = enRuWord.EnglishWord.EnWord,
          Russian = enRuWord.RussianWord.RuWord,
          Example = enRuWord.Example,
          Level = enRuWord.WordLevel
        };

        models.Add(viewModel);
      }

      return models;
    }

  }
}
