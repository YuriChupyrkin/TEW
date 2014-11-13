using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Domain.Entities;
using WpfUI.Auth;
using WpfUI.Helpers;

namespace WpfUI.Pages
{
  /// <summary>
  /// Interaction logic for SignIn.xaml
  /// </summary>
  public partial class SignIn : Page
  {
    public SignIn()
    {
      InitializeComponent();
      TxtLogin.Focus();
    }

    #region events

    private void BtnSignUp_Click(object sender, RoutedEventArgs e)
    {
      Switcher.Switch(new SignUpPage());
    }

    private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        StartSignIn();
      }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      StartSignIn();
    }

    #endregion

    #region methods

    private void StartSignIn()
    {
      try
      {
        var user = SignInUser();
        if (user == null)
        {
          throw new Exception("Sign in failed");
        }

        //if (MainWindow.IsOnlineVersion)
        //{
        //  var isSync = DialogHelper.YesNoQuestionDialog("Synchronize your data with cloud?", "Synchronize");
        //  if (isSync)
        //  {
        //    MainWindow.StartSync();
        //  }
        //}
        Dispatcher.BeginInvoke(DispatcherPriority.Background, new ThreadStart(IsStartSync));

        Switcher.Switch(new MainPage());
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Error");
      } 
    }

    public User SignInUser()
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

      var userProvider = new UserProvider(ApplicationContext.RepositoryFactory);

      User user = null;
      try
      {
        user = userProvider.ValidateUser(login, password);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }

      if (user != null)
      {
        ApplicationContext.CurrentUser = user;
      }
      else
      {
        throw new Exception("Incorrect login or password");
      }
      return user;
    }

    private void IsStartSync()
    {
      Thread.Sleep(1000);
      if (MainWindow.IsOnlineVersion)
      {
        var isSync = DialogHelper.YesNoQuestionDialog("Synchronize your data with cloud?", "Synchronize");
        if (isSync)
        {
          Switcher.Switch(new SyncPage());
        }
      }
    }
    #endregion
  }
}
