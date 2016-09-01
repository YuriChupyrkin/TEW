using System.Threading.Tasks;
using Domain.Entities;

namespace WpfUI.Services
{
	internal class UserDataProvider : DataProvider
	{
		public static async Task<User> SignUp(string email, string password)
		{
			var user = new User
			{
				Email = email,
				Password = password
			};

			return await SendRequestAsync<User, User>(user, SignUpWebController);
		}
	}
}
