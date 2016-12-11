using Domain.RepositoryFactories;
using EnglishLearnBLL.Models;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TewCloud.Helpers;

namespace TewCloud.Controllers.WebAppVersion
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
