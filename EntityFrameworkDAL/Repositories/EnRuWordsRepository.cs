using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Repositories;
using EntityFrameworkDAL.Context;

namespace EntityFrameworkDAL.Repositories
{
  public class EnRuWordsRepository : IEnRuWordsRepository
  {
    private readonly EnglishLearnContext _context;

    public EnRuWordsRepository(EnglishLearnContext context)
    {
      _context = context;
    }

    public IEnumerable<EnRuWord> AllEnRuWords()
    {
      return _context.EnRuWords.AsEnumerable();
    }

    public void DeleteEnRuWord(string enWord, int userId)
    {
      var enRuWord = _context.EnRuWords
        .FirstOrDefault(r => r.EnglishWord.EnWord == enWord && r.UserId == userId);
      if (enRuWord != null)
      {
        _context.EnRuWords.Remove(enRuWord);
        _context.SaveChanges();
      }
    }

    public void DeleteEnRuWord(int id)
    {
      var enRuWord = _context.EnRuWords.FirstOrDefault(r => r.Id == id);

      if (enRuWord != null)
      {
        _context.EnRuWords.Remove(enRuWord);
        _context.SaveChanges();
      }
    }

    public IEnumerable<string> GetTranslate(string enWord)
    {
      var russianWords = _context.EnRuWords
        .Where(r => r.EnglishWord.EnWord
          .Equals(enWord, StringComparison.OrdinalIgnoreCase))
        .Select(r => r.RussianWord.RuWord)
        .Distinct().AsEnumerable();

      return russianWords;
    }

    public EnRuWord AddTranslate(
      string engWord,
      string ruWord,
      string example,
      int userId,
      DateTime? updateDate = null,
      int level = 0,
      int answerCount = 0,
      int failAnswerCount = 1,
      bool isDeleted = false)
    {
      int engId = AddEngWord(engWord);
      int ruId = AddRusWord(ruWord);

      var enRuWordFromDb = _context.EnRuWords
        .FirstOrDefault(r => r.EnglishWord.EnWord == engWord && r.UserId == userId);

      if (enRuWordFromDb != null)
      {
        enRuWordFromDb.RussianWordId = ruId;
        enRuWordFromDb.WordLevel = level;
        enRuWordFromDb.Example = example ?? string.Empty;
        enRuWordFromDb.IsDeleted = isDeleted;
        enRuWordFromDb.UpdateDate = updateDate ?? new DateTime(1990, 5, 5);
        enRuWordFromDb.AnswerCount = answerCount;
        enRuWordFromDb.FailAnswerCount = failAnswerCount;
        _context.SaveChanges();

        return enRuWordFromDb;
      }

      var enRuWord = new EnRuWord
      {
        RussianWordId = ruId,
        EnglishWordId = engId,
        Example = example ?? string.Empty,
        UserId = userId,
        WordLevel = level,
        IsDeleted = isDeleted,
        UpdateDate = updateDate ?? new DateTime(1990, 5, 5),
        AnswerCount = answerCount,
        FailAnswerCount = failAnswerCount
      };

      _context.EnRuWords.Add(enRuWord);
      _context.SaveChanges();

      return enRuWord;
    }

    private int AddRusWord(string ruWord)
    {
      var russianWordFromDb = _context.RussianWords
        .FirstOrDefault(r => r.RuWord.Equals(ruWord, StringComparison.OrdinalIgnoreCase));

      if (russianWordFromDb == null)
      {
        ruWord = ruWord.Trim();
        var russianWord = new RussianWord
        {
          RuWord = ruWord.ToLower()
        };

        _context.RussianWords.Add(russianWord);
        _context.SaveChanges();

        return russianWord.Id;
      }

      return russianWordFromDb.Id;
    }

    private int AddEngWord(string enWord)
    {
      var engWordFromDb = _context.EnglishWords
        .FirstOrDefault(r => r.EnWord.Equals(enWord, StringComparison.OrdinalIgnoreCase));

      if (engWordFromDb == null)
      {
        enWord = enWord.Trim();
        var englishWord = new EnglishWord
        {
          EnWord = enWord.ToLower()
        };

        _context.EnglishWords.Add(englishWord);
        _context.SaveChanges();

        return englishWord.Id;
      }

      return engWordFromDb.Id;
    }

    public IDictionary<string, string> GetWordsForUser(int userId)
    {
      var enRuWords = _context.EnRuWords.Where(r => r.UserId == userId).AsEnumerable();

      var result = new Dictionary<string, string>();

      foreach (var enRuWord in enRuWords)
      {
        var enWord = enRuWord.EnglishWord;
        var ruWord = enRuWord.RussianWord;

        if (!string.IsNullOrEmpty(enWord.EnWord) && !string.IsNullOrEmpty(ruWord.RuWord))
        {
          result.Add(enWord.EnWord, ruWord.RuWord);
        }
      }
      return result;
    }


    public IEnumerable<RussianWord> AllRussianWords()
    {
      return _context.RussianWords.AsEnumerable();
    }

    public IEnumerable<EnglishWord> AllEnglishWords()
    {
      return _context.EnglishWords.AsEnumerable();
    }

    public void ChangeWordLevel(int enRuWordId, int levelShift)
    {
      var word = _context.EnRuWords.FirstOrDefault(r => r.Id == enRuWordId);

      if (word != null)
      {
        word.WordLevel = word.WordLevel + levelShift;
        word.UpdateDate = DateTime.UtcNow;
        word.AnswerCount++;

        if(levelShift < 0)
        {
          word.FailAnswerCount++;
        }

        _context.SaveChanges();
      }
    }

    public int ClearRuWords()
    {
      var ruWords = AllRussianWords();
      var count = 0;
      foreach (var word in ruWords)
      {
        if (string.IsNullOrWhiteSpace(word.RuWord))
        {
          _context.RussianWords.Remove(word);
          count++;
        }
        else if (word.EnRuWords.Any() == false)
        {
          _context.RussianWords.Remove(word);
          count++;
        }
      }
      _context.SaveChanges();
      return count;
    }

    public int ClearEnWords()
    {
      var enWords = AllEnglishWords();
      var count = 0;
      foreach (var word in enWords)
      {
        if (string.IsNullOrWhiteSpace(word.EnWord))
        {
          _context.EnglishWords.Remove(word);
          count++;
        }
        else if (word.EnRuWords.Any() == false)
        {
          _context.EnglishWords.Remove(word);
          count++;
        }
      }
      _context.SaveChanges();
      return count;
    }

    public void ResetWordLevel(int userId)
    {
      var userWords = AllEnRuWords().Where(r => r.UserId == userId);
      foreach (var userWord in userWords)
      {
        userWord.WordLevel = 0;
        userWord.UpdateDate = DateTime.UtcNow;
        userWord.AnswerCount = 0;
        userWord.FailAnswerCount = 1;
      }
      _context.SaveChanges();
    }

    public string GetRussianWordById(int id)
    {
      var rusWordObj = _context.RussianWords.Find(id);
      return rusWordObj == null ? "null" : rusWordObj.RuWord;
    }

    public string GetEnglishWordById(int id)
    {
      var engWordObj = _context.EnglishWords.Find(id);
      return engWordObj == null ? "null" : engWordObj.EnWord;
    }


    public EnRuWord MakeDeleted(string enWord, int userId)
    {
      var enRuWord = AllEnRuWords().FirstOrDefault(r => r.EnglishWord.EnWord == enWord && r.UserId == userId);

      if (enRuWord == null)
      {
        return null;
      }

      enRuWord.IsDeleted = true;
      enRuWord.UpdateDate = DateTime.UtcNow;
      _context.SaveChanges();

      return enRuWord;
    }

    public void RemoveAllDeletedWords(int userId)
    {
      var enRuWords = AllEnRuWords().Where(r => r.UserId == userId && r.IsDeleted);

      foreach (var word in enRuWords)
      {
        DeleteEnRuWord(word.Id);
      }
    }

  }
}