using System.Web.Http;
using Domain.RepositoryFactories;
using System.Linq;

namespace WebSite.Controllers.Api
{
  public class WordTranslaterController : ApiController
  {
    private readonly IRepositoryFactory _repositoryFactory;

    public WordTranslaterController(IRepositoryFactory repositoryFactory)
    {
      _repositoryFactory = repositoryFactory;
    }

    public IHttpActionResult GetWordTranslates(string word)
    {

      var translates = _repositoryFactory
        .EnRuWordsRepository.GetTranslate(word)
        .Select(
          r => new
          {
            Id = r.Id,
            UserId = r.UserId,
            English = r.EnglishWord.EnWord,
            Russian = r.RussianWord.RuWord,
            Example = r.Example,
            Level = r.WordLevel
          }
        );

      return Json(translates);
    }
  }
}