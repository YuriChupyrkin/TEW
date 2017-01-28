using System.Web.Http;
using Domain.RepositoryFactories;
using EnglishLearnBLL.WordLevelManager;
using WebSite.Models;

namespace WebSite.Controllers.Api
{
  public class UserStatController : ApiController
  {
    private readonly IRepositoryFactory _repositoryFactory;

    public UserStatController(IRepositoryFactory repositoryFactory)
    {
      _repositoryFactory = repositoryFactory;
    }

    [HttpGet]
    public IHttpActionResult GetUserStat(int userId)
    {
      var user = _repositoryFactory.UserRepository.Find(userId);

      if (user == null)
      {
        return NotFound();
      }

      var wordLevelManager = new WordLevelManager(_repositoryFactory);
      var wordLevel = wordLevelManager.GetUserWordsLevel(user.Id);

      var userStatModel = new UserStatModel
      {
        ActivityLevel = user.ActivityLevel,
        NickName = user.NickName,
        Email = user.Email,
        LastActivityDate = user.LastActivity.ToShortDateString(),
        WordsLevel = wordLevel
      };

      return Json(userStatModel);
    }
  }
}