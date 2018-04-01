using Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace EnglishLearnBLL.Tests
{
  internal class TestDataBuilder
  {
    private IEnumerable<EnRuWord> _wordCollection;

    public TestDataBuilder(IEnumerable<EnRuWord> wordCollection)
    {
      this._wordCollection = wordCollection;
    }

    private IEnumerable<EnRuWord> FilterByStudyPriority(
      IEnumerable<EnRuWord> wordsToFilter,
      int expectedSize,
      bool isLowerLevel = false
     )
    {
      IEnumerable<EnRuWord> result;

      if (isLowerLevel == false)
      {
        result = wordsToFilter
          .OrderBy(
             r =>
              (double)r.AnswerCount /
              (r.FailAnswerCount == 0 ? 1 : r.FailAnswerCount)
           )
          .OrderBy(r => r.WordLevel)
          .Take(expectedSize);
      }
      else
      {
        result = wordsToFilter
          .OrderBy(
            r =>
              (double)r.AnswerCount /
              (r.FailAnswerCount == 0 ? 1 : r.FailAnswerCount)
          )
          .OrderByDescending(r => r.WordLevel)
          .Take(expectedSize);
      }

      return result;
    }

    public IList<EnRuWord> GetTestData(int expectedSize, ITestTypes testType)
    {
      var enRuWords = this._wordCollection;

      if (enRuWords.Count() < expectedSize)
      {
        expectedSize = enRuWords.Count();

        if (expectedSize < 1)
        {
          return new List<EnRuWord>();
        }
      }

      var levelAppropriateWords = enRuWords
          .Where(
            r =>
              r.WordLevel >= testType.MinLevel &&
              r.WordLevel <= testType.MaxLevel
          );

      var testData = this.FilterByStudyPriority(
        levelAppropriateWords,
        expectedSize
      );

      if (testData.Count() < expectedSize)
      {
        var lowerLevelWords = enRuWords
          .Where(r => r.WordLevel < testType.MinLevel);
        var lowerLevelTestData = this.FilterByStudyPriority(
          lowerLevelWords,
          expectedSize - testData.Count(),
          true
        );

        testData = testData.Union(lowerLevelTestData);
      }

      return testData.ToList();
    }
  }
}
