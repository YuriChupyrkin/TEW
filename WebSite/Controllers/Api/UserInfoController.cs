using Domain.Entities;
using Domain.RepositoryFactories;
using Domain.RepositoryFactories.Models;
using EnglishLearnBLL.WordLevelManager;
using System.Linq;
using System.Web.Http;
using WebSite.Auth;
using WebSite.Models;

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
      var wordStat = wordLevelManager.GetUserWordsStat(user.Id);

      var userStatModel = new UserStatModel
      {
        NickName = user.NickName,
        Email = user.Email,
        WordsCount = wordStat.Item1,
        WordsLevel = wordStat.Item2,
        Id = user.Id,
        UniqueId = UserProvider.GetUserUniqueId(user.Id.ToString())
      };

      this.BuildWordStat(userStatModel);

      return Json(userStatModel);
    }

    private void BuildWordStat(UserStatModel userStatModel)
    {
      var words = _repositoryFactory.EnRuWordsRepository
        .AllEnRuWordsQueryable().Where(r => r.UserId == userStatModel.Id);

      var mostFailed = words
        .OrderByDescending(r => r.FailAnswerCount)
        .Where(r => r.FailAnswerCount > 0)
        .Take(10)
        .OrderBy(r => r.WordLevel)
        .Take(5)
        .Select(r => new SimpleWordModel
        {
          English = r.EnglishWord.EnWord,
          Russian = r.RussianWord.RuWord,
          FailAnswerCount = r.FailAnswerCount,
          Level = r.WordLevel,
          AnswerCount = r.AnswerCount
        });

      var mostStudied = words
        .OrderByDescending(r => r.WordLevel)
        .Where(r => r.WordLevel > 0)
        .Take(10)
        .OrderBy(r => r.FailAnswerCount)
        .Take(5)
        .Select(r => new SimpleWordModel
        {
          English = r.EnglishWord.EnWord,
          Russian = r.RussianWord.RuWord,
          FailAnswerCount = r.FailAnswerCount,
          Level = r.WordLevel,
          AnswerCount = r.AnswerCount
        });

      foreach (var failed in mostFailed)
      {
        var enlish = failed.English;
      }

      foreach (var studied in mostStudied)
      {
        var enlish = studied.English;
      }

      userStatModel.MostFailedWords = mostFailed.ToList();
      userStatModel.MostStudiedWords = mostStudied.ToList();
    }
  }
}
