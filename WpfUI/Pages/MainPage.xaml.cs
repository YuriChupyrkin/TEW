using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfUI.Helpers;

namespace WpfUI.Pages
{
  /// <summary>
  /// Interaction logic for MainPage.xaml
  /// </summary>
  public partial class MainPage : Page
  {
    public MainPage()
    {
      InitializeComponent();

      ApplicationValidator.ExpectAuthorized();
      LabelHello.Content = string
         .Format(" Hello {0}", ApplicationContext.CurrentUser.Email);
    }

    private void BtnAddWord_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      Switcher.Switch(new AddWordsPage());
    }

    private void BtnPikerTest_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      Switcher.Switch(new PickerTest());
    }

    private void BtnWriteTest_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      var writeTestPage = new WriteTestPage();
      Switcher.Switch(writeTestPage);
      writeTestPage.StartTest();
    }

    private void BtnMyWords_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      Switcher.Switch(new MyWordPage());
    }
  }
}
