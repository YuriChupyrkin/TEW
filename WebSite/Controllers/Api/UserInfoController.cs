using Domain.Entities;
using Domain.RepositoryFactories;
using EnglishLearnBLL.WordLevelManager;
using System.Web.Http;
using System.Web.Security;
using WebSite.Models;
using WebSite.Providers;

namespace WebSite.Controllers.Api
{
  public class UserInfoController : ApiController
  {
    private readonly IRepositoryFactory _repositoryFactory;

    public UserInfoController(IRepositoryFactory repositoryFactory)
    {
      _repositoryFactory = repositoryFactory;
    }

    public IHttpActionResult GetCurrentUserInfo(int userId)
    {
      User user;

      if (userId == 0)
      {
        var userEmail = User.Identity.Name;
        user = _repositoryFactory.UserRepository.Find(r => r.Email == userEmail);
      }
      else
      {
        user = _repositoryFactory.UserRepository.Find(userId);
      }

      var wordLevelManager = new WordLevelManager(_repositoryFactory);
      var wordLevel = wordLevelManager.GetUserWordsLevel(user.Id);

      var userStatModel = new UserStatModel
      {
        ActivityLevel = user.ActivityLevel,
        NickName = user.NickName,
        Email = user.Email,
        LastActivityDate = user.LastActivity.ToShortDateString(),
        WordsLevel = wordLevel,
        Id = user.Id
      };

      return Json(userStatModel);
    }
  }
}
