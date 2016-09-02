using System.Collections.Generic;
using System.Threading.Tasks;
using EnglishLearnBLL.Models;
using EnglishLearnBLL.WordLevelManager;

namespace WpfUI.Services
{
	internal class TestsDataProvider : DataProvider
	{
		public static async Task<List<PickerTestModel>> GetPickerTestModel(int userId, string testType)
		{
			var queryParams = new Dictionary<string, string>
			{
				{ "userId", userId.ToString() },
				{ "testType", testType }
			};

			return await SendGetRequestAsync<List<PickerTestModel>>(queryParams, PickerTestsController);
		}

		public static async Task<List<WriteTestModel>> GetWriteTestModel(int userId)
		{
			var queryParams = new Dictionary<string, string>
			{
				{ "userId", userId.ToString() }
			};

			return await SendGetRequestAsync<List<WriteTestModel>>(queryParams, WriteTestsController);
		}

		public static async Task<ResponseModel> UpdateWordLevel(
			bool isTrueAnswer, 
			int wordId,
			WordLevelManager.TestType testType)
		{
			var wordUpdateModel = new WordUpdateModel
			{
				WordId = wordId,
				IsTrueAnswer = isTrueAnswer,
				TestType = testType
			};

			return await SendPostRequestAsync<WordUpdateModel, ResponseModel>(wordUpdateModel, WordsLevelUpdaterController);
		}
	}
}
