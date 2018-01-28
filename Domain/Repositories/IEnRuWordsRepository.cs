using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Domain.Entities;
using System.Linq;
using Domain.RepositoryFactories.Models;

namespace Domain.Repositories
{
  public interface IEnRuWordsRepository : IGenericRepository
  {
    IQueryable<EnRuWord> AllEnRuWordsQueryable();
    IEnumerable<EnRuWord> GetTranslate(string enWord);
    EnRuWord AddTranslate(SimpleWordModel wordModel);
    IDictionary<string, string> GetWordsForUser(int userId);
    IEnumerable<EnRuWord> AllEnRuWords();
    IEnumerable<EnRuWord> AllEnRuWords(Expression<Func<EnRuWord, bool>> predicate);
    IEnumerable<RussianWord> AllRussianWords();
    IEnumerable<EnglishWord> AllEnglishWords();
    void ChangeWordLevel(int enRuWordId, int levelShift);
    void DeleteEnRuWord(string enWord, int userId);
    void EditEnRuWord(EnRuWord updateEnRuWord, int userId);
    void DeleteEnRuWord(int wordId);
    int ClearEnWords();
    int ClearRuWords();
    void ResetWordLevel(int userId);
    string GetRussianWordById(int id);
    string GetEnglishWordById(int id);
    void RemoveAllDeletedWords(int userId);
  }
}