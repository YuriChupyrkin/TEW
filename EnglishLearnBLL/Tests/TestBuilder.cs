using Domain.Entities;
using Domain.RepositoryFactories;
using EnglishLearnBLL.Models;
using System.Collections.Generic;
using System.Linq;

namespace EnglishLearnBLL.Tests
{
  public class TestBuilder
  {
    private int _userId;
    private int _testCollectionSize;
    private readonly IRepositoryFactory _repositoryFactory;

    public TestBuilder(
      IRepositoryFactory repositoryFactory,
      int userId,
      int testCollectionSize = 10
    )
    {
      this._userId = userId;
      this._testCollectionSize = testCollectionSize;
      this._repositoryFactory = repositoryFactory;
    }

    public IEnumerable<PickerTestModel> GetEnRuTestCollection()
    {
      var pickerTestBuilder = this.GetPickerTestBuilder();
      var testData = pickerTestBuilder.BuildPickerTest(new EnRuPickerTest(), 4);
      return testData;
    }

    public IEnumerable<PickerTestModel> GetRuEnTestCollection()
    {
      var pickerTestBuilder = this.GetPickerTestBuilder();
      var testData = pickerTestBuilder.BuildPickerTest(new RuEnPickerTest(), 4);
      return testData;
    }

    public IEnumerable<WriteTestModel> GetWriteTestCollection()
    {
      var userWords = this.GetUserWords(this._userId);
      var writeTestBuilder = new WriteTestBuilder(
        this._repositoryFactory,
        userWords,
        this._testCollectionSize
      );

      var testData = writeTestBuilder.BuildPickerTest(new WriteTest());
      return testData;
    }

    private PickerTestBuilder GetPickerTestBuilder()
    {
      var userWords = this.GetUserWords(this._userId);
      var pickerTestBuilder = new PickerTestBuilder(
        userWords,
        this._testCollectionSize
      );
 
      return pickerTestBuilder;
    }

    private IEnumerable<EnRuWord> GetUserWords(int userId)
    {
      var userWords = _repositoryFactory.EnRuWordsRepository
        .AllEnRuWords().Where(r => r.UserId == userId);

      return userWords;
    }
  }
}
