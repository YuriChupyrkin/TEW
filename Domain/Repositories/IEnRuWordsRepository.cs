using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Repositories
{
	public interface IEnRuWordsRepository : IGenericRepository
	{
		IEnumerable<string> GetTranslate(string enWord);

		EnRuWord AddTranslate(string engWord, string ruWord, string example, int userId, DateTime? updateDate = null,
			int level = 0, int answerCount = 0, int failAnswerCount = 1);

		IDictionary<string, string> GetWordsForUser(int userId);
		IEnumerable<EnRuWord> AllEnRuWords();
		IEnumerable<RussianWord> AllRussianWords();
		IEnumerable<EnglishWord> AllEnglishWords();
		void ChangeWordLevel(int enRuWordId, int levelShift);
		void DeleteEnRuWord(string enWord, int userId);
		void DeleteEnRuWord(int wordId);
		int ClearEnWords();
		int ClearRuWords();
		void ResetWordLevel(int userId);
		string GetRussianWordById(int id);
		string GetEnglishWordById(int id);
	  void RemoveAllDeletedWords(int userId);
	}
}