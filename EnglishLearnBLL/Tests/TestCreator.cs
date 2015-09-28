using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.RepositoryFactories;
using EnglishLearnBLL.Models;

namespace EnglishLearnBLL.Tests
{
  public class TestCreator
  {
    private const int WordCount = 10;
    private readonly IRepositoryFactory _repositoryFactory;

    public TestCreator(IRepositoryFactory repositoryFactory)
    {
      _repositoryFactory = repositoryFactory;
    }

    private IList<EnRuWord> Get10EnRuWords(int userId, int wordCount)
    {
      var enRuWords = _repositoryFactory.EnRuWordsRepository.AllEnRuWords()
        .Where(r => r.UserId == userId && r.IsDeleted == false).ToList();

      if (enRuWords.Count() < wordCount)
      {
        wordCount = enRuWords.Count();

        if (wordCount < 1)
        {
          return new List<EnRuWord>();
        }
      }

      var enRuWordsForTest = enRuWords
          .OrderBy(r => (double)r.AnswerCount / (r.FailAnswerCount == 0 ? 1 : r.FailAnswerCount))
          .OrderBy(r => r.WordLevel)
          .Take(wordCount).ToList();

      var maxLevel = enRuWordsForTest.Last().WordLevel;

      var wordsWithMaxLvlCount = enRuWords.Count(r => r.WordLevel == maxLevel);

      if (wordCount == WordCount)
      {
        var rnd = new Random();
        for (var i = 0; i < wordCount; i++)
        {
          if (enRuWordsForTest[i].WordLevel == maxLevel && wordsWithMaxLvlCount > 10)
          {
            enRuWordsForTest[i] = enRuWords
              .Where(r => r.WordLevel == maxLevel && !enRuWordsForTest.Contains(r))
              .OrderBy(r => rnd.Next()).ToList().First();
          }
        }
      }
      return enRuWordsForTest;
    }

    public IEnumerable<WriteTestModel> WriteTest(int userId)
    {
      var enRuWordsForTest = Get10EnRuWords(userId, WordCount);

      var writeTestModelList = new List<WriteTestModel>();

      if (!enRuWordsForTest.Any())
      {
        return writeTestModelList;
      }

      foreach (var enRuWord in enRuWordsForTest)
      {
        var ruWord = _repositoryFactory.EnRuWordsRepository
          .AllRussianWords().FirstOrDefault(r => r.Id == enRuWord.RussianWordId)
          .RuWord;

        var answerWord = _repositoryFactory.EnRuWordsRepository
          .AllEnglishWords().FirstOrDefault(r => r.Id == enRuWord.EnglishWordId)
          .EnWord;

        if (ruWord == null || answerWord == null)
        {
          throw new Exception("russian or english word is null");
        }

        var writeTestModel = new WriteTestModel
        {
          Word = ruWord,
          TrueAnswer = answerWord,
          EnRuWordId = enRuWord.Id,
          Example = enRuWord.Example
        };
        writeTestModelList.Add(writeTestModel);
      }

      return writeTestModelList;
    }

    public void SetWordLevelForWriteTest(WriteTestModel model)
    {
      if (model.IsAnswered)
      {
        _repositoryFactory.EnRuWordsRepository.ChangeWordLevel(model.EnRuWordId, 3);
      }
      else
      {
        _repositoryFactory.EnRuWordsRepository.ChangeWordLevel(model.EnRuWordId, -3);
      }
    }

    public IEnumerable<PickerTestModel> EnglishRussianTest(int userId)
    {
      var enRuWordsForTest = Get10EnRuWords(userId, WordCount);
      var wordCount = enRuWordsForTest.Count;

      var pickerModels = new List<PickerTestModel>();

      if (!enRuWordsForTest.Any())
      {
        return pickerModels;
      }

      var random = new Random();
      for (int i = 0; i < wordCount; i++)
      {
        var answerIndex = random.Next(4);

        var testedWord = _repositoryFactory.EnRuWordsRepository
          .AllEnglishWords()
          .FirstOrDefault(r => r.Id == enRuWordsForTest[i].EnglishWordId)
          .EnWord;

        var rusAnswer = _repositoryFactory.EnRuWordsRepository
          .AllRussianWords()
          .FirstOrDefault(r => r.Id == enRuWordsForTest[i].RussianWordId)
          .RuWord;

        if (testedWord == null || rusAnswer == null)
        {
          return pickerModels;
        }

        var randAnswer = Get4RandomRusWords(rusAnswer, i);

        var pickerTestModel = new PickerTestModel
        {
          AnswerId = answerIndex,
          Word = testedWord,
          Example = enRuWordsForTest[i].Example,
          WordId = enRuWordsForTest[i].Id,
          Answers = randAnswer.ToList()
        };
        pickerTestModel.Answers[answerIndex] = rusAnswer;

        pickerModels.Add(pickerTestModel);
      }

      return pickerModels;
    }

    public IEnumerable<PickerTestModel> RussianEnglishTest(int userId)
    {
      var enRuWordsForTest = Get10EnRuWords(userId, WordCount);
      var wordCount = enRuWordsForTest.Count;

      var pickerModels = new List<PickerTestModel>();

      if (!enRuWordsForTest.Any())
      {
        return pickerModels;
      }

      var random = new Random();
      for (int i = 0; i < wordCount; i++)
      {
        var answerIndex = random.Next(4);

        var engAnswer = _repositoryFactory.EnRuWordsRepository
          .AllEnglishWords()
          .FirstOrDefault(r => r.Id == enRuWordsForTest[i].EnglishWordId)
          .EnWord;

        var testedWord = _repositoryFactory.EnRuWordsRepository
          .AllRussianWords()
          .FirstOrDefault(r => r.Id == enRuWordsForTest[i].RussianWordId)
          .RuWord;

        if (testedWord == null || engAnswer == null)
        {
          return pickerModels;
        }

        var randAnswer = Get4RandomEngWords(engAnswer, i);

        var pickerTestModel = new PickerTestModel
        {
          AnswerId = answerIndex,
          Word = testedWord,
          WordId = enRuWordsForTest[i].Id,
          Answers = randAnswer.ToList(),
          Example = enRuWordsForTest[i].Example
        };
        pickerTestModel.Answers[answerIndex] = engAnswer;

        pickerModels.Add(pickerTestModel);
      }
      return pickerModels;
    }

    [Obsolete("Level up system was changed")]
    public string SetAnswerOnEnRuTest(PickerTestModel model, int answerIndex)
    {
      if (model.AnswerId == answerIndex)
      {
        _repositoryFactory.EnRuWordsRepository.ChangeWordLevel(model.WordId, 1);
      }
      else
      {
        _repositoryFactory.EnRuWordsRepository.ChangeWordLevel(model.WordId, -1);
      }
      return model.Answers[model.AnswerId];
    }

    [Obsolete("Level up system was changed")]
    public string SetAnswerOnRuEnTest(PickerTestModel model, int answerIndex)
    {
      if (model.AnswerId == answerIndex)
      {
        _repositoryFactory.EnRuWordsRepository.ChangeWordLevel(model.WordId, 2);
      }
      else
      {
        _repositoryFactory.EnRuWordsRepository.ChangeWordLevel(model.WordId, -2);
      }
      return model.Answers[model.AnswerId];
    }

    private IEnumerable<string> Get4RandomRusWords(string trueAnswer, int iteration)
    {
      var russianWords = _repositoryFactory.EnRuWordsRepository
        .AllEnRuWords()
        .Where(r => r.RussianWord.RuWord != trueAnswer)
        .OrderBy(r => (double)r.AnswerCount / (r.FailAnswerCount == 0 ? 1 : r.FailAnswerCount))
        .Select(r => r.RussianWord);

      if (russianWords.Count() < 4)
      {
        throw new Exception("Need more russian words in data base");
      }

      var skip = 3 * iteration;

      if (russianWords.Count() < skip + 4)
      {
        skip = russianWords.Count() - skip - 4;
      }

      if(skip < 0 || skip + 4 <= russianWords.Count())
      {
        skip = 0;
      }

      var rnd = new Random();
      var result = russianWords.Skip(skip).Take(4).Select(r => r.RuWord);

      return result;
    }

    private IEnumerable<string> Get4RandomEngWords(string trueAnswer, int iteration)
    {
      var engWords = _repositoryFactory.EnRuWordsRepository
        .AllEnRuWords()
        .Where(r => r.EnglishWord.EnWord != trueAnswer)
        .OrderBy(r => (double)r.AnswerCount / (r.FailAnswerCount == 0 ? 1 : r.FailAnswerCount))
        .Select(r => r.EnglishWord);

      if (engWords.Count() < 4)
      {
        throw new Exception("Need more english words in data base");
      }

      var skip = 3 * iteration;

      if (engWords.Count() < skip + 4)
      {
        skip = engWords.Count() - skip - 4;
      }

      if (skip < 0 || skip + 4 <= engWords.Count())
      {
        skip = 0;
      }

      var rnd = new Random();

      var result = engWords.Skip(skip).Take(4).Select(r => r.EnWord);

      return result;
    }
  }
}
