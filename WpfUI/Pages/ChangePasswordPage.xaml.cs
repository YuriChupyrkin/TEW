using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfUI.Helpers;
using WpfUI.Services;

namespace WpfUI.Pages
{
  /// <summary>
  /// Interaction logic for ChangePasswordPage.xaml
  /// </summary>
  public partial class ChangePasswordPage : Page
  {
    public ChangePasswordPage()
    {
      InitializeComponent();
      ApplicationValidator.ExpectAuthorized();
      TxtOldPassword.Focus();
    }

    private async void BtnSignUp_Click(object sender, RoutedEventArgs e)
    {
      await ChangePasswordAsync();
    }

    private async void TxtConfirmPassword_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
				await ChangePasswordAsync();
      }
    }

    private async Task ChangePasswordAsync()
    {
      if (ValidatePasswords() == false)
      {
        return;
      }

      var currentUser = ApplicationContext.CurrentUser;

	    var isChanged = await UserDataProvider.ChangePassword(currentUser.Email,
		    TxtOldPassword.Password, TxtPassword.Password);

			if (isChanged == false)
      {
        MessageBox.Show("Password change error", "Error");
        return;
      }

      MessageBox.Show("Password was changed successfully");
      Switcher.Switch(new MainPage());
    }

    private bool ValidatePasswords()
    {
      if (ApplicationValidator.IsValidatPassword(TxtOldPassword.Password) == false)
      {
        MessageBox.Show("Incorrect old password! (Min len = 4, Max len = 16");
        return false;
      }
      if (ApplicationValidator.IsValidatPassword(TxtPassword.Password) == false)
      {
        MessageBox.Show("Incorrect new password! (Min len = 4, Max len = 16");
        return false;
      }
      if (!ApplicationValidator.IsPasswordEquals(TxtPassword.Password, TxtConfirmPassword.Password))
      {
        MessageBox.Show("Password and confirm password must be equal");
        return false;
      }
      return true;
    }
  }
}
