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

		public static async Task<bool> ChangePassword(string email, string oldPassword, string newPassword)
		{
			var userChangePasswordModel = new ChangePasswordModel
			{
				Email = email,
				OldPassword = oldPassword,
				NewPassword = newPassword
			};

			return await SendPostRequestAsync<ChangePasswordModel, bool>(userChangePasswordModel, ChangePasswordController);
		}

		public static async Task<string> ResetPassword(string email)
		{
			return await SendPostRequestAsync<User, string>(new User { Email = email }, ResetPasswordController);
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
