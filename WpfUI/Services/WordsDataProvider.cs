using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using EnglishLearnBLL.Models;
using WpfUI.Helpers;

namespace WpfUI.Services
{
	internal class WordsDataProvider : DataProvider
	{
		public static async Task<WordsCloudModel> GetUserWordsAsync(string userName)
		{
			var queryStringParams = new Dictionary<string, string>
			{
				{ "userName", userName }
			};

			return await SendGetRequestAsync<WordsCloudModel>(queryStringParams, WordsManagerController);
		}

		public static async Task<ResponseModel> AddTranslateAsync(
			User user,
			string engWord,
			string ruWord,
			string example,
			DateTime updateDate)
		{
			var wordsCloudModel = MapperHelper.CreatWordJsonModel(user, engWord, ruWord, example, updateDate);

			return await SendPostRequestAsync<WordsCloudModel, ResponseModel>(wordsCloudModel, WordsManagerController);
		}

		public static async Task DeleteWordAsync(User user, string engWord)
		{
			var wordsCloudModel = MapperHelper.CreatWordJsonModel(user, engWord, null, null, DateTime.Now);
			await SendPostRequestAsync<WordsCloudModel, string>(wordsCloudModel, WordsManagerController, "DELETE");
		}
	}
}
