using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Common.Mail;
using Domain.Entities;
using WpfUI.Helpers;
using WpfUI.Services;

namespace WpfUI.Pages
{
	/// <summary>
	/// Interaction logic for SignUpPage.xaml
	/// </summary>
	public partial class SignUpPage : Page
	{
		public SignUpPage()
		{
			InitializeComponent();
			TxtLogin.Focus();
		}

		#region events

		private void BtnSignIn_Click(object sender, RoutedEventArgs e)
		{
			Switcher.Switch(new SignIn());
		}

		private async void TxtConfirmPassword_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				await StartSignUpAsync();
			}
		}

		private async void BtnSignUp_Click(object sender, RoutedEventArgs e)
		{
			await StartSignUpAsync();
		}

		#endregion

		#region methods

		private async Task StartSignUpAsync()
		{
			try
			{
				var user = await SignUpUserAsync();
				if (user == null)
				{
					throw new Exception("sign up failed");
				}
				MessageBox.Show("Welcome " + user.Email);

				Switcher.Switch(new MainPage());
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error");
			}
		}

		private async Task<User> SignUpUserAsync()
		{
			var login = TxtLogin.Text;

			if (!ApplicationValidator.IsValidatEmail(login))
			{
				throw new Exception("Incorrect email");
			}

			var password = TxtPassword.Password;

			if (!ApplicationValidator.IsValidatPassword(password))
			{
				throw new Exception("Incorrect password! (Min len = 4, Max len = 16)");
			}

			var confirmPassword = TxtConfirmPassword.Password;

			if (!ApplicationValidator.IsPasswordEquals(password, confirmPassword))
			{
				throw new Exception("Password and confirm password must be equal");
			}

			var user = await UserDataProvider.SignUpAsync(login, password);

			if (user == null)
			{
				throw new Exception("User already exist");
			}

			ApplicationContext.CurrentUser = user;
			return user;
		}

		private void SendEmailAboutRegistration(object newUser)
		{
			var userName = newUser as string;
			try
			{
				string emailMessage = string.Format("Hi!\nAdded new user: {0}", userName);
				IEmailSender emailSender = ApplicationContext.EmailSender;
				emailSender.Send(
					"socnetproject@yandex.ru",
					"socnetadmin",
					"yuri.chupyrkin@gmail.com",
					"TEW Newcomer",
					emailMessage);
			}
			catch
			{
				//it's ok...
			}
		}

		#endregion
	}
}
