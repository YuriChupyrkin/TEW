using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using EnglishLearnBLL.Models;
using EnglishLearnBLL.View;
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
			//ViewWords();
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
					// New web database logic
					await WordsDataProvider.DeleteWordAsync(ApplicationContext.CurrentUser, selectedWord.English);

					// Todo: remove and add new implementation
					/*
					var repositoryFactory = ApplicationContext.RepositoryFactory;

					var resultWord = repositoryFactory.EnRuWordsRepository.MakeDeleted(selectedWord.English, ApplicationContext.CurrentUser.Id);
					var syncHelper = new SynchronizeHelper();
					syncHelper.SendWordInBackGround(resultWord, ApplicationContext.CurrentUser);*/

					await ViewWordsAsync(); 
				}
			}
		}

		#endregion

		#region methods

		private async Task ViewWordsAsync()
		{
			DataGridWords.IsReadOnly = true;
			//var userId = ApplicationContext.CurrentUser.Id;
			var userName  = ApplicationContext.CurrentUser.Email;

			var wordViewer = new WordViewer(ApplicationContext.RepositoryFactory);

			var wordsCloudModel = await WordsDataProvider.GetUserWordsAsync(userName);
			var words = wordsCloudModel.Words;
			LabelMyWords.Content = TitlePage + ": " + wordsCloudModel.TotalWords;

			//var words = wordViewer.ViewWords(userId)
			//	.OrderByDescending(r => r.Level);
			//LabelMyWords.Content = TitlePage + ": " + words.Count();

			DataGridWords.ItemsSource = words;
		}

		#endregion
	}
}
