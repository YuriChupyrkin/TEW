using System.Threading.Tasks;
using System.Web.Http;
using Domain.Entities;
using Domain.RepositoryFactories;
using TewCloud.Auth;

namespace TewCloud.Controllers
{
	public class SignUpWebController : ApiController
	{
		private readonly IRepositoryFactory _repositoryFactory;

		public SignUpWebController(IRepositoryFactory repositoryFactory)
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
