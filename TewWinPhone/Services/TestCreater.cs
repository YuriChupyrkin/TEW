
using System;
using System.Collections.Generic;
using System.Linq;
using TewWinPhone.Entities;
using TewWinPhone.Models;

namespace TewWinPhone.Services
{
    internal sealed class TestCreater
    {
        private const int WordCount = 10;
        private readonly Random _random = new Random();

        public IEnumerable<PickerTestModel> CreateEnglishRussianTest(List<EnglishRussianWordEntity> userWords)
        {
            if(userWords == null || userWords.Count < 4)
            {
                return new List<PickerTestModel>();
            }

            var wordsForTest = Get10EnRuWords(userWords, WordCount);
            var pickerModes = new List<PickerTestModel>();

            foreach (var wordForTest in wordsForTest)
            {
                var answerIndex = _random.Next(4);

                var englishWord = wordForTest.English;
                var rusAnswer = wordForTest.Russian;

                var randomAnswers = GetRandomAnswers(userWords, wordForTest.Id);

                var pickerTestModel = new PickerTestModel
                {
                    AnswerId = answerIndex,
                    Example = wordForTest.ExampleOfUse,
                    Word = englishWord,
                    WordId = wordForTest.Id,
                    Answers = randomAnswers.Select(r => r.Russian).ToList()
                };

                pickerTestModel.Answers[answerIndex] = rusAnswer;
                pickerModes.Add(pickerTestModel);
            }

            return pickerModes;
        }

        public IEnumerable<PickerTestModel> CreateRussianEnglishTest(List<EnglishRussianWordEntity> userWords)
        {
            if (userWords == null || userWords.Count < 4)
            {
                return new List<PickerTestModel>();
            }

            var wordsForTest = Get10EnRuWords(userWords, WordCount);
            var pickerModes = new List<PickerTestModel>();

            foreach (var wordForTest in wordsForTest)
            {
                var answerIndex = _random.Next(4);

                var russianWord = wordForTest.Russian;
                var engAnswer = wordForTest.English;

                var randomAnswers = GetRandomAnswers(userWords, wordForTest.Id);

                var pickerTestModel = new PickerTestModel
                {
                    AnswerId = answerIndex,
                    Example = wordForTest.ExampleOfUse,
                    Word = russianWord,
                    WordId = wordForTest.Id,
                    Answers = randomAnswers.Select(r => r.English).ToList()
                };

                pickerTestModel.Answers[answerIndex] = engAnswer;
                pickerModes.Add(pickerTestModel);
            }

            return pickerModes;
        }

        private IEnumerable<EnglishRussianWordEntity> GetRandomAnswers(List<EnglishRussianWordEntity> userWords, int wordId)
        {
            var answers = userWords.Where(r => r.Id != wordId).OrderBy(r => _random.Next()).Take(4);

            return answers;
        }

        private IList<EnglishRussianWordEntity> Get10EnRuWords(List<EnglishRussianWordEntity> userWords, int wordCount)
        {
            if (userWords.Count < wordCount)
            {
                wordCount = userWords.Count;

                if (wordCount < 1)
                {
                    return new List<EnglishRussianWordEntity>();
                }
            }
           
            var enRuWordsForTest = userWords.Where(r => r.IsDeleted == false).OrderBy(r => r.WordLevel).Take(wordCount).ToList();
            var maxLevel = enRuWordsForTest.Last().WordLevel;

            var wordsWithMaxLvlCount = userWords.Count(r => r.WordLevel == maxLevel);

            if (wordCount == WordCount)
            {
                for (var i = 0; i < wordCount; i++)
                {
                    if (enRuWordsForTest[i].WordLevel == maxLevel && wordsWithMaxLvlCount > 10)
                    {
                        enRuWordsForTest[i] = userWords
                          .Where(r => r.WordLevel == maxLevel && !enRuWordsForTest.Contains(r))
                          .OrderBy(r => _random.Next()).ToList().First();
                    }
                }
            }
            return enRuWordsForTest;
        }
    }
}
