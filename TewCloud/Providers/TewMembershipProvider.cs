using System.Web.Mvc;
using System.Web.Security;
using Domain.RepositoryFactories;
using TewCloud.Auth;
namespace TewCloud.Providers
{
	public class TewMembershipProvider : MembershipProvider
	{
		private readonly UserProvider _userProvider;

		public TewMembershipProvider()
		{
			_userProvider = new UserProvider(RepositoryFactory);
		}

		public IRepositoryFactory RepositoryFactory
		{
			get
			{
				return (IRepositoryFactory) DependencyResolver.Current.GetService(typeof (IRepositoryFactory));
			}
		}

		public bool CreateUser(string email, string password)
		{
			var user = _userProvider.CreateUser(email, password);

			return user != null;
		}

		public override bool ChangePassword(string username, string oldPassword, string newPassword)
		{
			return _userProvider.ChangePassword(username, oldPassword, newPassword);
		}

		public override string ResetPassword(string username, string answer)
		{
			return _userProvider.ResetPassword(username);
		}

		public override bool ValidateUser(string username, string password)
		{
			var user = _userProvider.ValidateUser(username, password);
			return user != null;
		}

		// Not necessary
		public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
		{
			throw new System.NotImplementedException();
		}

		public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
		{
			throw new System.NotImplementedException();
		}

		public override MembershipUser GetUser(string username, bool userIsOnline)
		{
			throw new System.NotImplementedException();
		}

		public override string ApplicationName
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
				throw new System.NotImplementedException();
			}
		}

		public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
		{
			throw new System.NotImplementedException();
		}

		public override bool DeleteUser(string username, bool deleteAllRelatedData)
		{
			throw new System.NotImplementedException();
		}

		public override bool EnablePasswordReset
		{
			get { throw new System.NotImplementedException(); }
		}

		public override bool EnablePasswordRetrieval
		{
			get { throw new System.NotImplementedException(); }
		}

		public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			throw new System.NotImplementedException();
		}

		public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			throw new System.NotImplementedException();
		}

		public override int GetNumberOfUsersOnline()
		{
			throw new System.NotImplementedException();
		}

		public override string GetPassword(string username, string answer)
		{
			throw new System.NotImplementedException();
		}

		public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
		{
			throw new System.NotImplementedException();
		}

		public override string GetUserNameByEmail(string email)
		{
			throw new System.NotImplementedException();
		}

		public override int MaxInvalidPasswordAttempts
		{
			get { throw new System.NotImplementedException(); }
		}

		public override int MinRequiredNonAlphanumericCharacters
		{
			get { throw new System.NotImplementedException(); }
		}

		public override int MinRequiredPasswordLength
		{
			get { throw new System.NotImplementedException(); }
		}

		public override int PasswordAttemptWindow
		{
			get { throw new System.NotImplementedException(); }
		}

		public override MembershipPasswordFormat PasswordFormat
		{
			get { throw new System.NotImplementedException(); }
		}

		public override string PasswordStrengthRegularExpression
		{
			get { throw new System.NotImplementedException(); }
		}

		public override bool RequiresQuestionAndAnswer
		{
			get { throw new System.NotImplementedException(); }
		}

		public override bool RequiresUniqueEmail
		{
			get { throw new System.NotImplementedException(); }
		}

		public override bool UnlockUser(string userName)
		{
			throw new System.NotImplementedException();
		}

		public override void UpdateUser(MembershipUser user)
		{
			throw new System.NotImplementedException();
		}
	}
}