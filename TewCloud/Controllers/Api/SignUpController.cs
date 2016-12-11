using System.Web.Http;
using Domain.Entities;
using Domain.RepositoryFactories;
using TewCloud.Auth;

namespace TewCloud.Controllers.Api
{
	public class SignUpController : ApiController
	{
		private readonly IRepositoryFactory _repositoryFactory;

		public SignUpController(IRepositoryFactory repositoryFactory)
		{
			_repositoryFactory = repositoryFactory;
		}

		[HttpPost]
		public IHttpActionResult CreateNewUser([FromBody] User user)
		{
			var userProvider = new UserProvider(_repositoryFactory);
			var createdUser = userProvider.CreateUser(user.Email, user.Password);

			return Json(createdUser);
		}
	}
}
