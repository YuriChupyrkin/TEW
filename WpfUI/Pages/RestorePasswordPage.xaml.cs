using System;
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
  ///   Interaction logic for RestorePasswordPage.xaml
  /// </summary>
  public partial class RestorePasswordPage : Page
  {
    public RestorePasswordPage()
    {
      InitializeComponent();
      TxtEmail.Focus();
    }

    private void BtnRestorePsw_Click(object sender, RoutedEventArgs e)
    {
      Restore();
    }

    private void TxtEmail_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        Restore();
      }
    }

    private void Restore()
    {
      if (MainWindow.IsOnlineVersion == false)
      {
        MessageBox.Show("You must have Internet connect");
        return;
      }

      string email = TxtEmail.Text;
      var userProvider = new UserProvider(ApplicationContext.RepositoryFactory);

      if (ValidateEmail(email, userProvider) == false)
      {
        return;
      }

      string newPassword = userProvider.ResetPassword(email);

      try
      {
        string emailMessage = string.Format("Hi!\nYour new password: {0}", newPassword);
        IEmailSender emailSender = ApplicationContext.EmailSender;
        emailSender.Send(
          "socnetproject@yandex.ru",
          "socnetadmin",
          email,
          "You have changed password on TEW",
          emailMessage);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Email send message error");
        MainWindow.SetAdminAuthentication();
        return;
      }
      MessageBox.Show("Your new password sent on email");
      Switcher.Switch(new SignIn());
    }

    private bool ValidateEmail(string email, UserProvider userProvider)
    {
      bool isValidEmail = ApplicationValidator.IsValidatEmail(email);
      if (!isValidEmail)
      {
        MessageBox.Show("Invalid email");
        return false;
      }

      User user = userProvider.GetUser(email);
      if (user == null)
      {
        MessageBox.Show("User not exist");
        return false;
      }
      return true;
    }
  }
}