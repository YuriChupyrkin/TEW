using System.Linq;
using Domain.Entities;
using Domain.RepositoryFactories;

namespace EnglishLearnBLL.WordLevelManager
{
  public sealed class WordLevelManager
  {
    public enum TestType
    {
      EnRuTest,
      RuEnTest,
      SpellingWithHelpTest,
      SpellingTest
    }

    private const int EnRuMaxLevel = 5;
    private const int RuEnMaxLevel = 10;
    private const int SpelWithHelpMaxLevel = 15;
    private const int SpelMaxLevel = 20;

    private readonly IRepositoryFactory _repositoryFactory;

    public WordLevelManager(IRepositoryFactory repositoryFactory)
    {
      _repositoryFactory = repositoryFactory;
    }

    public void SetWordLevel(int wordId, bool isRight, TestType testType)
    {
      EnRuWord word = _repositoryFactory.EnRuWordsRepository.AllEnRuWords()
        .FirstOrDefault(r => r.Id == wordId);

      if (word == null)
      {
        return;
      }

      if (isRight)
      {
        switch (testType)
        {
          case TestType.EnRuTest:
            LevelUp(word, EnRuMaxLevel);
            break;
          case TestType.RuEnTest:
            LevelUp(word, RuEnMaxLevel);
            break;
          case TestType.SpellingWithHelpTest:
            LevelUp(word, SpelWithHelpMaxLevel);
            break;
          case TestType.SpellingTest:
            LevelUp(word, SpelMaxLevel);
            break;
        }
      }
      else
      {
        if (word.WordLevel > 0)
        {
          _repositoryFactory.EnRuWordsRepository.ChangeWordLevel(word.Id, -1);
        }
      }
    }

    private void LevelUp(EnRuWord word, int maxLevel)
    {
      if (word.WordLevel < maxLevel)
      {
        _repositoryFactory.EnRuWordsRepository.ChangeWordLevel(word.Id, 1);
      }
    }

  }
}
