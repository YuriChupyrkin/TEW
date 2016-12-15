using System;
using System.Web.Mvc;
using Domain.RepositoryFactories;
using EnglishLearnBLL.Models;
using Newtonsoft.Json;
using TewCloud.Helpers;

namespace WebSite.Controllers
{
  [Authorize]
  public class WordsUploaderController : Controller
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly UserHelper _userHelper;

    public WordsUploaderController(IRepositoryFactory repositoryFactory)
    {
      _repositoryFactory = repositoryFactory;
      _userHelper = new UserHelper(_repositoryFactory);
    }

    public ActionResult Index()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Index(string wordsJson)
    {
      try
      {
        WordsFullModel wordsModel = JsonConvert.DeserializeObject<WordsFullModel>(wordsJson);

        if (wordsModel == null)
        {
          throw new Exception("Invalid JSON");
        }

        if (string.IsNullOrEmpty(wordsModel.UserName))
        {
          throw new Exception("Invalid JSON (invalid user name)");
        }

        var currentUser = HttpContext.User.Identity.Name;

        if (currentUser.ToLower() != wordsModel.UserName.ToLower())
        {
          throw new Exception("Words should be for current user");
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
        ViewBag.Error = ex.Message;
        return View();
      }

      ViewBag.Success = "Uploaded...";
      return View();
    }
  }
}