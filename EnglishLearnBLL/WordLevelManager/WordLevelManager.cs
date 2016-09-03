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

        public EnRuWord SetWordLevel(int wordId, bool isRight, TestType testType)
        {
            EnRuWord word = _repositoryFactory.EnRuWordsRepository.AllEnRuWords()
              .FirstOrDefault(r => r.Id == wordId);

            if (word == null)
            {
                return null;
            }

            if (isRight)
            {
                switch (testType)
                {
                    case TestType.EnRuTest:
                        word = LevelUp(word, EnRuMaxLevel);
                        break;
                    case TestType.RuEnTest:
                        word = LevelUp(word, RuEnMaxLevel);
                        break;
                    case TestType.SpellingWithHelpTest:
                        word = LevelUp(word, SpelWithHelpMaxLevel);
                        break;
                    case TestType.SpellingTest:
                        word = LevelUp(word, SpelMaxLevel);
                        break;
                }
            }
            else
            {
                _repositoryFactory.EnRuWordsRepository.ChangeWordLevel(word.Id, -1);
            }
            return word;
        }

        private EnRuWord LevelUp(EnRuWord word, int maxLevel)
        {
            if (word.WordLevel < maxLevel)
            {
                _repositoryFactory.EnRuWordsRepository.ChangeWordLevel(word.Id, 1);
                //word.WordLevel++;
            }

            return word;
        }

    }
}
