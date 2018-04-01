using Domain.Entities;
using Domain.RepositoryFactories;
using EnglishLearnBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnglishLearnBLL.Tests
{
  internal class WriteTestBuilder
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private IEnumerable<EnRuWord> _wordsCollection;
    private int _testCollectionSize;

    public WriteTestBuilder(
      IRepositoryFactory repositoryFactory,
      IEnumerable<EnRuWord> wordsCollection,
      int testCollectionSize
    )
    {
      this._repositoryFactory = repositoryFactory;
      this._wordsCollection = wordsCollection;
      this._testCollectionSize = testCollectionSize;
    }

    public IEnumerable<WriteTestModel> BuildPickerTest(ITestTypes testType)
    {
      var testData = GetTestData(new WriteTest());
      var writeTestModelList = new List<WriteTestModel>();

      if (!testData.Any())
      {
        return writeTestModelList;
      }

      foreach (var testDataItem in testData)
      {
        var ruWord = _repositoryFactory.EnRuWordsRepository
          .AllRussianWords()
          .FirstOrDefault(r => r.Id == testDataItem.RussianWordId)
          .RuWord;

        var answerWord = _repositoryFactory.EnRuWordsRepository
          .AllEnglishWords()
          .FirstOrDefault(r => r.Id == testDataItem.EnglishWordId)
          .EnWord;

        if (ruWord == null || answerWord == null)
        {
          throw new Exception("russian or english word is null");
        }

        var writeTestModel = new WriteTestModel
        {
          Word = ruWord,
          TrueAnswer = answerWord,
          EnRuWordId = testDataItem.Id,
          Example = testDataItem.Example
        };
        writeTestModelList.Add(writeTestModel);
      }

      return writeTestModelList;
    }

    private IList<EnRuWord> GetTestData(ITestTypes testType)
    {
      var testDataBulder = new TestDataBuilder(this._wordsCollection);
      var testData = testDataBulder
        .GetTestData(this._testCollectionSize, testType);

      return testData;
    }
  }
}
