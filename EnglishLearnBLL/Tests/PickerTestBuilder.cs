
using Domain.Entities;
using EnglishLearnBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnglishLearnBLL.Tests
{
  internal class PickerTestBuilder
  {
    private IEnumerable<EnRuWord> _wordsCollection;
    private int _testCollectionSize;

    public PickerTestBuilder(
      IEnumerable<EnRuWord> wordsCollection,
      int testCollectionSize
    )
    {
      this._wordsCollection = wordsCollection;
      this._testCollectionSize = testCollectionSize;
    }

    public IEnumerable<PickerTestModel> BuildPickerTest(
      ITestTypes testType,
      int anserOptionsCount
    )
    {
      var testData = this.GetTestData(testType);
      var wordCount = testData.Count;
      var pickerModels = new List<PickerTestModel>();

      if (!testData.Any())
      {
        return pickerModels;
      }

      var random = new Random();
      for (int i = 0; i < wordCount; i++)
      {
        var answerIndex = random.Next(anserOptionsCount);

        var answer = testType.IsEnRu ?
          testData[i].RussianWord.RuWord :
          testData[i].EnglishWord.EnWord;

        var testWord = testType.IsEnRu ?
          testData[i].EnglishWord.EnWord :
          testData[i].RussianWord.RuWord;

        if (testWord == null || answer == null)
        {
          return pickerModels;
        }

        var randAnswer = GetAnswerOptions(
          answer,
          i,
          this._wordsCollection,
          testType.IsEnRu,
          anserOptionsCount
        );

        var pickerTestModel = new PickerTestModel
        {
          AnswerId = answerIndex,
          Word = testWord,
          WordId = testData[i].Id,
          Answers = randAnswer.ToList(),
          Example = testData[i].Example
        };

        pickerTestModel.Answers[answerIndex] = answer;

        pickerModels.Add(pickerTestModel);
      }

      return pickerModels;
    }

    private IList<EnRuWord> GetTestData(ITestTypes testType)
    {
      var testDataBulder = new TestDataBuilder(this._wordsCollection);
      var testData = testDataBulder
        .GetTestData(this._testCollectionSize, testType);

      return testData;
    }

    private IEnumerable<string> GetAnswerOptions(
      string trueAnswer,
      int iteration,
      IEnumerable<EnRuWord> wordsCollection,
      bool isEnRuTest,
      int anserOptionsCount = 4
    )
    {
      IEnumerable<Entity<int>> answers;
      var random = new Random();

      if (isEnRuTest)
      {
        var words = wordsCollection
          .Where(r => r.RussianWord.RuWord != trueAnswer)
          .OrderBy(
            r =>
              (double)r.AnswerCount /
              (r.FailAnswerCount == 0 ? 1 : r.FailAnswerCount)
            );

        answers = words.Select(r => r.RussianWord);
      }
      else
      {
        var words = wordsCollection
          .Where(r => r.EnglishWord.EnWord != trueAnswer)
          .OrderBy(
            r =>
              (double)r.AnswerCount /
              (r.FailAnswerCount == 0 ? 1 : r.FailAnswerCount)
            );

        answers = words.Select(r => r.EnglishWord);
      }

      if (answers.Count() < anserOptionsCount)
      {
        throw new Exception("Need more english words in data base");
      }

      var skip = anserOptionsCount - 1 * iteration;

      if (answers.Count() < skip + anserOptionsCount)
      {
        skip = random.Next(0, answers.Count() - anserOptionsCount);
      }

      if (isEnRuTest)
      {
        return ((IEnumerable<RussianWord>)answers)
          .Skip(skip)
          .Take(anserOptionsCount)
          .Select(r => r.RuWord);
      }
      else
      {
        return ((IEnumerable<EnglishWord>)answers)
          .Skip(skip)
          .Take(anserOptionsCount)
          .Select(r => r.EnWord);
      }
    }
  }
}
