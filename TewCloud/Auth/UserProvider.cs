using System;
using System.Text;
using Domain.Entities;
using Domain.RepositoryFactories;
using Domain.UnitOfWork;

namespace TewCloud.Auth
{
	internal class UserProvider
	{
		private const string SaltWord = "SaltWord";

		private readonly IUnitOfWork _unitOfWork;
		private readonly IRepositoryFactory _repositoryFactory;

		public UserProvider(IRepositoryFactory repositoryFactory)
		{
			_repositoryFactory = repositoryFactory;
			_unitOfWork = (IUnitOfWork)_repositoryFactory;
		}

		public User GetUser(string login)
		{
			return _repositoryFactory.UserRepository.Find(r => r.Email.Equals(login, StringComparison.OrdinalIgnoreCase));
		}

		public User CreateUser(string login, string password)
		{
			var user = GetUser(login);
			if (user == null)
			{
				try
				{
					user = new User();
					user.Email = login;
					user.Password = HashPassword(password);

					var role = _repositoryFactory
						.RoleRepository.Find(x => x.RoleName == "user");

					if (role != null)
					{
						user.RoleId = role.Id;
						_repositoryFactory.UserRepository.Create(user);
						_unitOfWork.Commit();

						return user;
					}
					throw new NullReferenceException("Role not found!");
				}
				catch (Exception ex)
				{
					throw new Exception(ex.Message);
				}
			}
			return null;
		}

		public User ValidateUser(string username, string password)
		{
			try
			{
				User user = _repositoryFactory.UserRepository
					.Find(x => x.Email.Equals(username, StringComparison.OrdinalIgnoreCase));

				if (user == null)
				{
					return null;
				}

				ChangePasswordIfItHttpCrypto(user);

				if (VerifyHashedPassword(user.Password, password))
				{
					return user;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return null;
		}

		public string ResetPassword(string username)
		{
			try
			{
				var user = _repositoryFactory.UserRepository
						.Find(x => x.Email.ToLower() == username.ToLower());

				if (user != null)
				{
					var newPassword = RandomString(6);
					user.Password = HashPassword(newPassword);
					_unitOfWork.Commit();
					return newPassword;
				}
				else
				{
					throw new NullReferenceException("User not exist");
				}
			}
			catch (NullReferenceException ex)
			{
				throw new NullReferenceException(ex.Message);
			}
		}

		public bool ChangePassword(string username, string oldPassword, string newPassword)
		{
			var isChanged = false;
			try
			{
				var user = _repositoryFactory.UserRepository.
						Find(x => x.Email.ToLower() == username.ToLower());
				if (user != null && VerifyHashedPassword(user.Password, oldPassword))
				{
					user.Password = HashPassword(newPassword);
					_unitOfWork.Commit();
					isChanged = true;
				}
			}
			catch
			{
				isChanged = false;
			}
			return isChanged;
		}

		private string RandomString(int length)
		{
			var random = new Random(Environment.TickCount);
			var chars = "0123456789abcdefghijklmnopqrstuvwxyz";
			var builder = new StringBuilder(length);

			for (int i = 0; i < length; ++i)
				builder.Append(chars[random.Next(chars.Length)]);
			return builder.ToString();
		}

		private string HashPassword(string password)
		{
			var passHash = password.GetHashCode();
			var saltHash = SaltWord.GetHashCode();

			var result = passHash + saltHash;
			return result.ToString();
		}

		private bool VerifyHashedPassword(string originalPassword, string password)
		{
			var passHash = password.GetHashCode();
			var saltHash = SaltWord.GetHashCode();

			var resultPass = (passHash + saltHash).ToString();
			return originalPassword == resultPass;
		}

		// Note: we used System.Web.Http for crypto. It obsoleted. 
		private void ChangePasswordIfItHttpCrypto(User user)
		{
			var userPassword = user.Password;

			int passwordInt;

			var tryParseResult = int.TryParse(userPassword, out passwordInt);

			if (tryParseResult)
			{
				return;
			}

			var newPassword = user.Email.Substring(0, 4);
			var newPasswordHash = HashPassword(newPassword);

			var userFromDb = _repositoryFactory.UserRepository.Find(x => x.Email.ToLower() == user.Email.ToLower());

			if (userFromDb != null)
			{
				userFromDb.Password = newPasswordHash;
				_unitOfWork.Commit();

				throw new Exception(string.Format("Warning! We have some updates. You have got a new password: {0}. Please change it", newPassword));
			}
		}
	}
}