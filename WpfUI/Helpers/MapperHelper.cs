using System;
using Domain.Entities;
using EnglishLearnBLL.Models;
namespace WpfUI.Helpers
{
	internal class MapperHelper
	{
		public static WordsCloudModel CreatWordJsonModel(
			User user, 
			string engWord, 
			string ruWord, 
			string example, 
			DateTime updateDate)
		{
			var cloudModel = new WordsCloudModel { UserName = user.Email };

			var viewModel = new WordJsonModel
			{
				English = engWord,
				Russian = ruWord,
				Example = example,
				UpdateDate = updateDate
			};

			cloudModel.Words.Add(viewModel);

			return cloudModel;
		}
	}
}
