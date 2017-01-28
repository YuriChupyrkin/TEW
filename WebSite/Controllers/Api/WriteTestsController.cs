using System.Web.Http;
using Domain.RepositoryFactories;
using EnglishLearnBLL.Tests;
using TewCloud.FIlters;

namespace WebSite.Controllers.Api
{
  [UserActivityFilter]
  public class WriteTestsController : ApiController
	{
		private readonly IRepositoryFactory _repositoryFactory;

		public WriteTestsController(IRepositoryFactory repositoryFactory)
		{
			_repositoryFactory = repositoryFactory;
		}

		public IHttpActionResult GetWriteTestSet(int userId)
		{
			var testCreator = new TestCreator(_repositoryFactory);
			var testSet = testCreator.WriteTest(userId);

			return Json(testSet);
		}
	}
}
