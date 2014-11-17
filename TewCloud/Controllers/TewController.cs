using System.Web.Mvc;
using Domain.Entities;
using Domain.RepositoryFactories;
using Domain.UnitOfWork;

namespace TewCloud.Controllers
{
  public class TewController : Controller
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IUnitOfWork _unitOfWork;

    public TewController(IRepositoryFactory repositoryFactory)
    {
      _repositoryFactory = repositoryFactory;
      _unitOfWork = (IUnitOfWork) repositoryFactory;
    }

    public ActionResult Index()
    {
      return View();
    }

  }
}