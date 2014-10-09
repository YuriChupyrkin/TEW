using System.Linq;
using System.Web.Http;
using Domain.RepositoryFactories;
using Domain.UnitOfWork;
using TewCloud.Models;

namespace TewCloud.Controllers
{
  public class TewInfoController : ApiController
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IUnitOfWork _unitOfWork;

    public TewInfoController(IRepositoryFactory repositoryFactory)
    {
      _repositoryFactory = repositoryFactory;
      _unitOfWork = (IUnitOfWork) repositoryFactory;
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