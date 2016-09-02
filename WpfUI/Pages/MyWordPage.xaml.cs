using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using EnglishLearnBLL.Models;
using WpfUI.Helpers;
using WpfUI.Services;

namespace WpfUI.Pages
{
	/// <summary>
	/// Interaction logic for MyWordPage.xaml
	/// </summary>
	public partial class MyWordPage : Page
	{
		private const string TitlePage = "My words";

		public MyWordPage()
		{
			InitializeComponent();
			ApplicationValidator.ExpectAuthorized();
		}

		#region events

		private async void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			await ViewWordsAsync();
		}

		private async void DataGridWords_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var selectedWord = DataGridWords.SelectedValue as WordViewModel;

			if (selectedWord != null)
			{
				var isDelete = DialogHelper
					.YesNoQuestionDialog("Delete word \"" + selectedWord.English + "\"?", "Deleting");

				if (isDelete)
				{
					await WordsDataProvider.DeleteWordAsync(ApplicationContext.CurrentUser, selectedWord.English);
					await ViewWordsAsync();
				}
			}
		}

		#endregion

		#region methods

		private async Task ViewWordsAsync()
		{
			DataGridWords.IsReadOnly = true;
			var userName = ApplicationContext.CurrentUser.Email;

			var wordsCloudModel = await WordsDataProvider.GetUserWordsAsync(userName);
			var words = wordsCloudModel.Words;
			LabelMyWords.Content = TitlePage + ": " + wordsCloudModel.TotalWords;

			DataGridWords.ItemsSource = words;
		}

		#endregion
	}
}
