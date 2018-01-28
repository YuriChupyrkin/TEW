using System.Web.Http;
using Domain.RepositoryFactories;
using WebSite.Auth;
using TewCloud.FIlters;
using WebSite.Models;

namespace WebSite.Controllers.Api
{
  [UserActivityFilter]
  public class ChangePasswordController : ApiController
  {
    private readonly IRepositoryFactory _repositoryFactory;

    public ChangePasswordController(IRepositoryFactory repositoryFactory)
    {
      _repositoryFactory = repositoryFactory;
    }

    [HttpPost]
    public IHttpActionResult ChangePassword(ChangePasswordModel changePasswordModel)
    {
      var userProvider = new UserProvider(_repositoryFactory);

      var isChanged = userProvider.ChangePassword(changePasswordModel.Email,
        changePasswordModel.OldPassword, changePasswordModel.NewPassword);

      return Json(isChanged);
    }
  }
}
