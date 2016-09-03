using System.Web.Http;
using Domain.Entities;
using Domain.RepositoryFactories;
using TewCloud.Auth;

namespace TewCloud.Controllers.WebAppVersion
{
	public class ResetPasswordController : ApiController
	{
		private readonly IRepositoryFactory _repositoryFactory;

		public ResetPasswordController(IRepositoryFactory repositoryFactory)
		{
			_repositoryFactory = repositoryFactory;
		}

		[HttpPost]
		public IHttpActionResult ResetPassword([FromBody] User user)
		{
			var userProvider = new UserProvider(_repositoryFactory);
			string newPassword = userProvider.ResetPassword(user.Email);

			return Json(newPassword);
		}
	}
}
