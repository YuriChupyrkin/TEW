using System.Linq;
using System.Web.Http;
using Domain.RepositoryFactories;
using EnglishLearnBLL.Models;
using WebSite.Helpers;

namespace WebSite.Controllers.Api
{
  public class DeleteWordController : ApiController
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly UserHelper _userHelper;

    public DeleteWordController(IRepositoryFactory repositoryFactory)
    {
      _repositoryFactory = repositoryFactory;
      _userHelper = new UserHelper(repositoryFactory);
    }

    [HttpPost]
    public IHttpActionResult DeleteWord(WordsFullModel wordsModel)
    {
      if (wordsModel != null && wordsModel.Words.Any())
      {
        var userId = _userHelper.GetUserId(wordsModel.UserName);
        _repositoryFactory.EnRuWordsRepository.DeleteEnRuWord(wordsModel.Words.First().English, userId);

        return Ok();
      }

      return NotFound();
    }
  }
}
