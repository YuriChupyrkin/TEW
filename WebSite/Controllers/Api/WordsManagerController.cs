using System;
using System.Web.Http;
using Domain.RepositoryFactories;
using EnglishLearnBLL.Models;
using TewCloud.FIlters;
using WebSite.Helpers;
using WebSite.Models;

namespace WebSite.Controllers.Api
{
  [UserActivityFilter]
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
    public IHttpActionResult AddWords([FromBody] WordsFullModel wordsModel)
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
          modelItem.UserId = userId;
          _repositoryFactory.EnRuWordsRepository.AddTranslate(modelItem);
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
        WordsCloudModel = new WordsFullModel
        {
          TotalWords = _userHelper.GetWordCount(wordsModel.UserName)
        }
      };

      return Json(okResponse);
    }

    [HttpGet]
    public IHttpActionResult GetWords([FromUri] GetUserWordsModel getUserWordsModel)
    {
      WordsFullModel cloudModel;
      try
      {
        cloudModel = _userHelper.GetUserWords(getUserWordsModel);
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
