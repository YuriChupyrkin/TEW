using Domain.RepositoryFactories;
using EnglishLearnBLL.Models;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using TewCloud.Helpers;

namespace TewCloud.Controllers
{
  public class SynchronizeController : ApiController
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly SyncHelper _syncHelper;

    public SynchronizeController(IRepositoryFactory repositoryFactory)
    {
      _repositoryFactory = repositoryFactory;
      _syncHelper = new SyncHelper(repositoryFactory);
    }

    [HttpPost]
    public async Task<IHttpActionResult> AddWords([FromBody] WordsCloudModel wordsModel)
    {
      try
      {
        if (wordsModel == null)
        {
          throw new ArgumentException("words model is null");
        }

        var userId = _syncHelper.GetUserId(wordsModel.UserName);

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
          TotalWords = _syncHelper.GetWordCount(wordsModel.UserName)
        }
      };
      return Json(okResponse);
    }

    [HttpGet]
    public IHttpActionResult GetWords(string UserName, long UpdateDate)
    {
      UserUpdateDateModel updateModel = new UserUpdateDateModel
      {
        UpdateDate = UpdateDate,
        UserName = UserName
      };

      WordsCloudModel cloudModel;
      try
      {
        cloudModel = _syncHelper.GetUserWords(updateModel);

        var userId = _repositoryFactory
          .UserRepository
          .Find(r => r.Email.Equals(UserName, StringComparison.OrdinalIgnoreCase))
          .Id;

        _repositoryFactory.EnRuWordsRepository.RemoveAllDeletedWords(userId);
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
  }
}
