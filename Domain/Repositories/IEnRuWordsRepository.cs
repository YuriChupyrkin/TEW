﻿using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Repositories
{
  public interface IEnRuWordsRepository : IGenericRepository
  {
    IEnumerable<string> GetTranslate(string enWord);
    void AddTranslate(string engWord, string ruWord, string context, int userId);
    IDictionary<string, string> GetWordsForUser(int userId);
    IEnumerable<EnRuWord> AllEnRuWords();
    IEnumerable<RussianWord> AllRussianWords();
    IEnumerable<EnglishWord> AllEnglishWords();
    void ChangeWordLevel(int enRuWordId, int levelShift);
    void DeleteEnRuWord(string enWord, int userId);
    int ClearEnWords();
    int ClearRuWords();
    void ResetWordLevel(int userId);
  }
}