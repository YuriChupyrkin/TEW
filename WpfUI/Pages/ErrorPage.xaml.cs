using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfUI.Pages
{
  /// <summary>
  /// Interaction logic for ErrorPage.xaml
  /// </summary>
  public partial class ErrorPage : Page
  {
    private Page _redirectPage;
    public ErrorPage()
    {
      InitializeComponent();
      LabelError.Content = "Error...";
      ButtonName();
    }

    public ErrorPage(string errorMessage)
    {
      InitializeComponent();
      LabelError.Content = string.Format(
        "Error! {0}", errorMessage);
      ButtonName();
    }

    private void BtnLoginMenu_Click(object sender, RoutedEventArgs e)
    {
      Switcher.Switch(_redirectPage);
    }

    private void ButtonName()
    {
      if (ApplicationContext.CurrentUser == null)
      {
        BtnLoginMenu.Content = "Login menu";
        _redirectPage = new SignIn();
      }
      else
      {
        BtnLoginMenu.Content = "Menu";
        _redirectPage = new MainPage();
      }
    }
    
  }
}
