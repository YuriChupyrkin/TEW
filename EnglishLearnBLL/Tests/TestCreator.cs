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
    private readonly Random _random;

    public TestCreator(IRepositoryFactory repositoryFactory)
    {
      _repositoryFactory = repositoryFactory;
      _random = new Random();
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
			return PickerTest(userId, true);
		}

		public IEnumerable<PickerTestModel> RussianEnglishTest(int userId)
		{
			return PickerTest(userId, false);
		}

		private IList<EnRuWord> Get10EnRuWords(int userId, int wordCount)
		{
			var enRuWords = _repositoryFactory.EnRuWordsRepository.AllEnRuWords()
				.Where(r => r.UserId == userId).ToList();

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

		private IEnumerable<PickerTestModel> PickerTest(int userId, bool isEnRuTest)
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

                var answer = isEnRuTest ? enRuWordsForTest[i].RussianWord.RuWord : enRuWordsForTest[i].EnglishWord.EnWord;
                var tested = isEnRuTest ? enRuWordsForTest[i].EnglishWord.EnWord : enRuWordsForTest[i].RussianWord.RuWord;

				if (tested == null || answer == null)
				{
					return pickerModels;
				}

				var randAnswer = Get4RandonWords(answer, i, userId, isEnRuTest);

				var pickerTestModel = new PickerTestModel
				{
					AnswerId = answerIndex,
					Word = tested,
					WordId = enRuWordsForTest[i].Id,
					Answers = randAnswer.ToList(),
					Example = enRuWordsForTest[i].Example
				};

				pickerTestModel.Answers[answerIndex] = answer;

				pickerModels.Add(pickerTestModel);
			}

			return pickerModels;
		}

		private IEnumerable<string> Get4RandonWords(string trueAnswer, int iteration, int userId, bool isEnRuTest)
		{
      IEnumerable<Entity<int>> answers;

      if (isEnRuTest)
      {
          var words = _repositoryFactory.EnRuWordsRepository.AllEnRuWords().Where(r => r.UserId == userId)
              .Where(r => r.RussianWord.RuWord != trueAnswer)
              .OrderBy(r => (double)r.AnswerCount / (r.FailAnswerCount == 0 ? 1 : r.FailAnswerCount));

          answers = words.Select(r => r.RussianWord);
      }
      else
      {
          var words = _repositoryFactory.EnRuWordsRepository.AllEnRuWords().Where(r => r.UserId == userId)
              .Where(r => r.EnglishWord.EnWord != trueAnswer)
              .OrderBy(r => (double)r.AnswerCount / (r.FailAnswerCount == 0 ? 1 : r.FailAnswerCount));

          answers = words.Select(r => r.EnglishWord);
      }

      if (answers.Count() < 4)
      {
          throw new Exception("Need more english words in data base");
      }

      var skip = 3 * iteration;

      if (answers.Count() < skip + 4)
      {
        skip = _random.Next(0, answers.Count() - 4);
      }

      if (isEnRuTest)
      {
        return ((IEnumerable<RussianWord>)answers).Skip(skip).Take(4).Select(r => r.RuWord);
      }
      else
      {
        return ((IEnumerable<EnglishWord>)answers).Skip(skip).Take(4).Select(r => r.EnWord);
      }
		}
	}
}
