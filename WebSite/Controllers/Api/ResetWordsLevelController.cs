using System.Web.Http;
using Domain.Entities;
using Domain.RepositoryFactories;
using TewCloud.FIlters;

namespace WebSite.Controllers.Api
{
  [UserActivityFilter]
  public class ResetWordsLevelController : ApiController
  {
    private readonly IRepositoryFactory _repositoryFactory;

    public ResetWordsLevelController(IRepositoryFactory repositoryFactory)
    {
      _repositoryFactory = repositoryFactory;
    }

    [HttpPost]
    public IHttpActionResult ResetWordsLevel(User user)
    {
      if (user == null)
      {
        return NotFound();
      }

      _repositoryFactory.EnRuWordsRepository.ResetWordLevel(user.Id);
      return Json(true);
    }
  }
}
