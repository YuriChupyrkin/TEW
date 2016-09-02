using System.Web.Http;
using Domain.RepositoryFactories;
using EnglishLearnBLL.Models;
using EnglishLearnBLL.WordLevelManager;

namespace TewCloud.Controllers.WebAppVersion
{
	public class WordsLevelUpdaterController : ApiController
	{
		private readonly IRepositoryFactory _repositoryFactory;

		public WordsLevelUpdaterController(IRepositoryFactory repositoryFactory)
		{
			_repositoryFactory = repositoryFactory;
		}

		[HttpPost]
		public IHttpActionResult UpdateWordStatus([FromBody] WordUpdateModel wordUpdateModel)
		{
			var wordLevelManager = new WordLevelManager(_repositoryFactory);

			var resultWord = wordLevelManager.SetWordLevel(
				wordUpdateModel.WordId, 
				wordUpdateModel.IsTrueAnswer, wordUpdateModel.TestType);

			var responseModel = new ResponseModel();

			if (resultWord == null)
			{
				responseModel.IsError = true;
				responseModel.ErrorMessage = "Error occurred! Word was not updated";
			}

			return Json(responseModel);
		}
	}
}