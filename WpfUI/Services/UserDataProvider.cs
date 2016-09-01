using System.Threading.Tasks;
using Domain.Entities;

namespace WpfUI.Services
{
	internal class UserDataProvider : DataProvider
	{
		public static async Task<User> SignUpAsync(string email, string password)
		{
			return await SendPostRequestAsync<User, User>(BuildUserByEmailAndPass(email, password), SignUpController);
		}

		public static async Task<User> SignInAsync(string email, string password)
		{
			return await SendPostRequestAsync<User, User>(BuildUserByEmailAndPass(email, password), SignInController);
		}

		private static User BuildUserByEmailAndPass(string email, string password)
		{
			return new User
			{
				Email = email,
				Password = password
			};
		}
	}
}
