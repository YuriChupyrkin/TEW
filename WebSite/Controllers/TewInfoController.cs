using System.Linq;
using System.Web.Http;
using Domain.RepositoryFactories;
using WebSite.Models;

namespace WebSite.Controllers
{
  public class TewInfoController : ApiController
  {
    private readonly IRepositoryFactory _repositoryFactory;

    public TewInfoController(IRepositoryFactory repositoryFactory)
    {
      _repositoryFactory = repositoryFactory;
    }

    [HttpGet]
    public IHttpActionResult GetTewInfo()
    {
      var usersCount = _repositoryFactory.UserRepository.All().Count();
      var wordsCount = _repositoryFactory.EnRuWordsRepository.AllEnRuWords().Count();

      var infoModel = new TewInfoModel
      {
        Users = usersCount,
        Words = wordsCount
      };

      return Json(infoModel);
    }
  }
}