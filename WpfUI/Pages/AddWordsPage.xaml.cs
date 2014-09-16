using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Domain.RepositoryFactories;
using WpfUI.Helpers;

namespace WpfUI.Pages
{
  /// <summary>
  /// Interaction logic for AddWordsPage.xaml
  /// </summary>
  public partial class AddWordsPage : Page
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private const string MyTranslate = "My translate";
   
    public AddWordsPage()
    {
      InitializeComponent();

      ApplicationValidator.ExpectAuthorized();

      TxtRusTranslate.Visibility = Visibility.Hidden;
      BtnAdd.IsEnabled = false;
 
      _repositoryFactory = ApplicationContext.RepositoryFactory;
      TxtEnglishWord.Focus();
      BtnSearch.IsEnabled = false;
    }

    #region events

    private async void BtnSearch_Click(object sender, RoutedEventArgs e)
    {
      await Search();
    }

    private void ListTranslate_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      var item = ItemsControl
        .ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;
      
      if (item != null)
      {
        if (item.Content.ToString().Equals(MyTranslate) == false)
        {
          AddWord(item.Content.ToString());
        }
      }
    }

    private void TxtEnglishWord_TextChanged(object sender, TextChangedEventArgs e)
    {
      var textBox = sender as TextBox;

      if (textBox.Text.Length > 0)
      {
        ListTranslate.Items.Clear();
        TxtRusTranslate.Clear();
        TxtRusTranslate.Visibility = Visibility.Hidden;
        BtnSearch.IsEnabled = true;
      }
      else
      {
        BtnSearch.IsEnabled = false;
      }
    }

    private void BtnAdd_Click(object sender, RoutedEventArgs e)
    {
      AddWord(TxtRusTranslate.Text);
    }

    private void TxtRusTranslate_TextChanged(object sender, TextChangedEventArgs e)
    {
      var textBox = sender as TextBox;

      if (textBox.Text.Length > 1)
      {
        BtnAdd.IsEnabled = true;
      }
      else
      {
        BtnAdd.IsEnabled = false;
      }
    }

    private async void TxtEnglishWord_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        await Search();
      }
    }

    private void TxtRusTranslate_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        AddWord(TxtRusTranslate.Text);
      }
    }

    private void TxtEnglishWord_GotFocus(object sender, RoutedEventArgs e)
    {
      InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("EN-us");
    }

    private void TxtRusTranslate_GotFocus(object sender, RoutedEventArgs e)
    {
      InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("RU-ru");
    }

    private void ListTranslate_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      var item = ItemsControl
        .ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;

      if (item != null)
      {
        if (item.Content.ToString().Equals(MyTranslate))
        {
          TxtRusTranslate.Visibility = Visibility.Visible;
          TxtRusTranslate.Focus();
        }
        else
        {
          TxtRusTranslate.Visibility = Visibility.Hidden;
          TxtRusTranslate.Text = string.Empty;
        }
      }
    }

    #endregion

    #region methods
    private void AddWord(string rusWord)
    {
      var enWord = TxtEnglishWord.Text;
      var addMessage = string.Format("Add translate: [{0}] <=> [{1}]?", enWord, rusWord);
      var isAdd = DialogHelper.YesNoQuestionDialog(addMessage, string.Empty);

      if (isAdd == false)
      {
        return;
      }

      try
      {
        var userId = ApplicationContext.CurrentUser.Id;
        _repositoryFactory.EnRuWordsRepository.AddTranslate(enWord, rusWord, userId);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
      ClearInputs();
      TxtEnglishWord.Focus();
    }

    private void ClearInputs()
    {
      TxtEnglishWord.Text = string.Empty;
      TxtRusTranslate.Text = string.Empty;
      TxtRusTranslate.Visibility = Visibility.Hidden;
      ListTranslate.Items.Clear();
    }

    private async Task Search()
    {
      ListTranslate.Items.Clear();

      var word = TxtEnglishWord.Text;
      var translates = _repositoryFactory.EnRuWordsRepository.GetTranslate(word);

      ListTranslate.Items.Add(MyTranslate);
      foreach (var translate in translates)
      {
        ListTranslate.Items.Add(translate);
      }

      var bingTranslate = await AddTranslateFromBing();

      if (translates.Contains(bingTranslate) == false && bingTranslate.Length > 0)
      {
        ListTranslate.Items.Add(bingTranslate);
      }
    }

    private async Task<string> AddTranslateFromBing()
    {
      if (MainWindow.IsOnlineVersion == false)
      {
        return string.Empty;
      }

      if (ApplicationContext.AdminAuthentication == null)
      {
        MainWindow.SetAdminAuthentication();
        return string.Empty;
      }

      var translate = await ApplicationContext
        .BingTranslater.GetTranslate(TxtEnglishWord.Text);

      return translate.ToLower();
    }

    #endregion
  }
}
