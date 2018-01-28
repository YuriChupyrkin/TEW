using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Domain.RepositoryFactories;
using EnglishLearnBLL.Models;
using EnglishLearnBLL.Tests;
using EnglishLearnBLL.WordLevelManager;
using TewCloud.FIlters;

namespace WebSite.Controllers.Api
{
  [UserActivityFilter]
  public class PickerTestsController : ApiController
  {
    private readonly IRepositoryFactory _repositoryFactory;

    public PickerTestsController(IRepositoryFactory repositoryFactory)
    {
      _repositoryFactory = repositoryFactory;
    }

    public IHttpActionResult GetPickerTestSet(int userId, string testType)
    {
      var testCreator = new TestCreator(_repositoryFactory);
      IEnumerable<PickerTestModel> testSet;

      if (testType == WordLevelManager.TestType.EnRuTest.ToString())
      {
        testSet = testCreator.EnglishRussianTest(userId).ToList();
      }
      else
      {
        testSet = testCreator.RussianEnglishTest(userId).ToList();
      }

      return Json(testSet);
    }
  }
}