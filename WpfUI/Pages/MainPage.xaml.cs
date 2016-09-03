using System.Linq;
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

    private async void BtnWriteTest_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      var writeTestPage = new WriteTestPage();
      Switcher.Switch(writeTestPage);
      await writeTestPage.StartTestAsync();
    }

    private void BtnMyWords_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      Switcher.Switch(new MyWordPage());
    }

    private void Grid_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
			// TODO! WHAT IS IT?!
			/*
      var userId = ApplicationContext.CurrentUser.Id;
      var userWords = ApplicationContext.RepositoryFactory
        .EnRuWordsRepository.AllEnRuWords().Where(r => r.UserId == userId).ToList();

      var totalPoint = userWords.Sum(r => r.WordLevel);

      LabelPoints.Content = totalPoint;
			*/
    }
  }
}
