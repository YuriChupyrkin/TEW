using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfUI.Helpers;
using WpfUI.Services;

namespace WpfUI.Pages
{
  /// <summary>
  /// Interaction logic for AddWordsPage.xaml
  /// </summary>
  public partial class AddWordsPage : Page
  {
    private readonly GoogleTranslater _googleTranslator;
    private const string MyTranslate = "My translate";

    private string _selectedTranslate;

    public AddWordsPage()
    {
      InitializeComponent();

      ApplicationValidator.ExpectAuthorized();

      TxtRusTranslate.IsEnabled = false;
      BtnAdd.IsEnabled = false;

      _googleTranslator = new GoogleTranslater();

      TxtEnglishWord.Focus();
      BtnSearch.IsEnabled = false;
      labelExist.Content = string.Empty;
    }

    #region events

    private async void BtnSearch_Click(object sender, RoutedEventArgs e)
    {
      await SearchAsync();
    }

    private async void ListTranslate_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      var item = ItemsControl
        .ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;

      if (item != null)
      {
        if (item.Content.ToString().Equals(MyTranslate) == false)
        {
          await AddWordAsync(item.Content.ToString());
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
        TxtRusTranslate.IsEnabled = false;
        BtnSearch.IsEnabled = true;
      }
      else
      {
        BtnSearch.IsEnabled = false;
      }
    }

    private async void BtnAdd_Click(object sender, RoutedEventArgs e)
    {
      if (string.IsNullOrEmpty(_selectedTranslate))
      {
        await AddWordAsync(TxtRusTranslate.Text);
      }
      else
      {
				await AddWordAsync(_selectedTranslate);
      }
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
        await SearchAsync();
      }
    }

    private async void TxtRusTranslate_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
				await AddWordAsync(TxtRusTranslate.Text);
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

    private void TxtRusTranslate_LostFocus(object sender, RoutedEventArgs e)
    {
      InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("EN-us");
    }

    private void ListTranslate_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      var item = ItemsControl
        .ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;

      if (item != null)
      {
        if (item.Content.ToString().Equals(MyTranslate))
        {
          TxtRusTranslate.IsEnabled = true;
          BtnAdd.IsEnabled = false;
          _selectedTranslate = string.Empty;
          TxtRusTranslate.Focus();
        }
        else
        {
          TxtRusTranslate.IsEnabled = false;
          TxtRusTranslate.Text = string.Empty;
          _selectedTranslate = item.Content.ToString();
          BtnAdd.IsEnabled = true;
        }
      }
    }

    #endregion

    #region methods

    private async Task AddWordAsync(string rusWord)
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
        string example = TxtExample.Text;

        const int maxLen = 500;

        if (example.Length > maxLen)
        {
          example = example.Substring(0, maxLen);
          example += "...";
        }
				
				var responseModel = 
					await WordsDataProvider.AddTranslateAsync(ApplicationContext.CurrentUser, enWord, rusWord, example, DateTime.UtcNow);

				if (responseModel.IsError)
				{
					throw new Exception("Word was not added! " + responseModel.ErrorMessage);
				}
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
      TxtRusTranslate.IsEnabled = false;
      BtnAdd.IsEnabled = false;
      ListTranslate.Items.Clear();
      TxtExample.Clear();
    }

    private async Task SearchAsync()
    {
      TxtExample.Clear();
      ListTranslate.Items.Clear();
      labelExist.Content = string.Empty;

      var word = TxtEnglishWord.Text;
	    var translates = await WordsDataProvider.GetTranslates(word);

			ListTranslate.Items.Add(MyTranslate);

      foreach (var translate in translates)
      {
        ListTranslate.Items.Add(translate);
      }

      //var googleTranslate = await AddTranslateFromGoogle();

      //foreach (var translate in googleTranslate)
      //{
      //  if (translates.Contains(translate) == false && translate.Length > 0)
      //  {
      //    ListTranslate.Items.Add(translate);
      //  }
      //}
    }

		// TODO! USE IT
    private async Task<string[]> AddTranslateFromGoogle()
    {
      var result = await _googleTranslator.GetTranslate(TxtEnglishWord.Text);

      TxtExample.AppendText(result.Example);
      return result.Translates;
    }

    #endregion
  }
}
