using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Common.Mail;
using WpfUI.Services;

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

    private async void BtnRestorePsw_Click(object sender, RoutedEventArgs e)
    {
			await RestoreAsync();
    }

    private async void TxtEmail_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
				await RestoreAsync();
      }
    }

    private async Task RestoreAsync()
    {
      string email = TxtEmail.Text;

	    string newPassword = await UserDataProvider.ResetPassword(email);

	    if (string.IsNullOrEmpty(newPassword))
	    {
		    MessageBox.Show("User does not exist");
		    return;
	    }

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
        MessageBox.Show("New password: " + newPassword);
        return;
      }

      MessageBox.Show("Your new password sent on email");
      Switcher.Switch(new SignIn());
    }
  }
}