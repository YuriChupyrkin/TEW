using System.Web.Http;
using Domain.RepositoryFactories;
using EnglishLearnBLL.Tests;
using TewCloud.FIlters;
using System;

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
      var testCreator = new TestBuilder(_repositoryFactory, userId, 10);
      var testSet = testCreator.GetWriteTestCollection();

      return Json(testSet);
    }
  }
}
