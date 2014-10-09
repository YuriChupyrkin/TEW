using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Common.Mail;
using Domain.Entities;
using WpfUI.Auth;
using WpfUI.Helpers;

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

    private void TxtConfirmPassword_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        StartSignUp();
      }
    }

    private void BtnSignUp_Click(object sender, RoutedEventArgs e)
    {
      StartSignUp();
    }

    #endregion

    #region methods

    private void StartSignUp()
    {
      try
      {
        var user = SignUpUser();
        if (user == null)
        {
          throw new Exception("sign up failed");
        }
        MessageBox.Show("Welcome " + user.Email);

        //var thread = new Thread(SendEmailAboutRegistration);
        //thread.Start(user.Email);
        System.Diagnostics.Process.Start(SynchronizeHelper.Uri);

        Switcher.Switch(new MainPage());
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Error");
      }
    }

    private User SignUpUser()
    {
      var login = TxtLogin.Text;

      if (!ApplicationValidator.IsValidatEmail(login))
      {
        throw new Exception("Incorrect email");
      }

      var password = TxtPassword.Password;

      if (!ApplicationValidator.IsValidatPassword(password))
      {
        throw new Exception("Incorrect password! (Min len = 4, Max len = 16");
      }

      var confirmPassword = TxtConfirmPassword.Password;

      if (!ApplicationValidator.IsPasswordEquals(password, confirmPassword))
      {
        throw new Exception("Password and confirm password must be equal");
      }

      var user = CreateUser(login, password);
      if (user == null)
      {
        throw new Exception("User already exist");
      }

      ApplicationContext.CurrentUser = user;
      return user;
    }

    private User CreateUser(string login, string password)
    {
      var userProvider = new UserProvider(ApplicationContext.RepositoryFactory);
      return userProvider.CreateUser(login, password);
    }

    private void SendEmailAboutRegistration(object newUser)
    {
      if (MainWindow.IsOnlineVersion == false)
      {
        return;
      }

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
