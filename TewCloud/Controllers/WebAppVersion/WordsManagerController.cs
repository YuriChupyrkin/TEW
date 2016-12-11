using System;
using System.Linq;
using System.Web.Http;
using Domain.RepositoryFactories;
using EnglishLearnBLL.Models;
using TewCloud.Helpers;

namespace TewCloud.Controllers.WebAppVersion
{
	public class WordsManagerController : ApiController
	{
		private readonly IRepositoryFactory _repositoryFactory;
		private readonly UserHelper _userHelper;

		public WordsManagerController(IRepositoryFactory repositoryFactory)
		{
			_repositoryFactory = repositoryFactory;
      _userHelper = new UserHelper(repositoryFactory);
		}

		[HttpPost]
		public IHttpActionResult AddWords([FromBody] WordsCloudModel wordsModel)
		{
			try
			{
				if (wordsModel == null)
				{
					throw new ArgumentException("words model is null");
				}

				var userId = _userHelper.GetUserId(wordsModel.UserName);

				foreach (var modelItem in wordsModel.Words)
				{
					_repositoryFactory.EnRuWordsRepository
						.AddTranslate(
							modelItem.English,
							modelItem.Russian,
							modelItem.Example,
							userId,
							modelItem.UpdateDate,
							modelItem.Level,
							modelItem.AnswerCount,
							modelItem.FailAnswerCount);
				}
			}
			catch (Exception ex)
			{
				var response = new ResponseModel
				{
					IsError = true,
					ErrorMessage = ex.Message
				};

				return Json(response);
			}

			var okResponse = new ResponseModel
			{
				IsError = false,
				ErrorMessage = string.Empty,
				WordsCloudModel = new WordsCloudModel
				{
					TotalWords = _userHelper.GetWordCount(wordsModel.UserName)
				}
			};

			return Json(okResponse);
		}

		[HttpGet]
		public IHttpActionResult GetWords(string userName)
		{
			UserUpdateDateModel updateModel = new UserUpdateDateModel
			{
				UserName = userName
			};

			WordsCloudModel cloudModel;
			try
			{
				cloudModel = _userHelper.GetUserWords(updateModel);
			}
			catch (Exception ex)
			{
				var response = new ResponseModel
				{
					IsError = true,
					ErrorMessage = ex.Message
				};

				return Json(response);
			}

			return Json(cloudModel);
		}

		[HttpDelete]
		public void DeleteWord([FromBody] WordsCloudModel wordsModel)
		{
			if (wordsModel != null && wordsModel.Words.Any())
			{
				var userId = _userHelper.GetUserId(wordsModel.UserName);
				_repositoryFactory.EnRuWordsRepository.DeleteEnRuWord(wordsModel.Words.First().English, userId);
			}
		}
	}
}
