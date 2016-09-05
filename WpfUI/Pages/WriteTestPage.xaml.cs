using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EnglishLearnBLL.Models;
using EnglishLearnBLL.WordLevelManager;
using WpfUI.Helpers;
using WpfUI.Services;

namespace WpfUI.Pages
{
	/// <summary>
	/// Interaction logic for WriteTestPage.xaml
	/// </summary>
	public partial class WriteTestPage : Page
	{
		private List<WriteTestModel> _testSet;
		private int _testIndex;
		private int _testCount;
		private int _failedCount;
		private bool _isHelpOn;

		public WriteTestPage()
		{
			InitializeComponent();

			ApplicationValidator.ExpectAuthorized();
		}

		public async Task StartTestAsync()
		{
			_testSet = await TestsDataProvider.GetWriteTestModel(ApplicationContext.CurrentUser.Id);
			_testCount = _testSet.Count;
			_testIndex = 0;
			_failedCount = 0;
			PrintCurrentTest();
		}

		#region events

		private async void BtnAnswer_Click(object sender, RoutedEventArgs e)
		{
			if (TxtAnwer.Text.Length < 1)
			{
				return;
			}

			await CheckAnswerAsync();
			await TestIndexIncrementAsync();
		}

		private async void TxtAnwer_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key != Key.Enter)
			{
				return;
			}
			if (TxtAnwer.Text.Length < 1)
			{
				return;
			}

			await CheckAnswerAsync();
			await TestIndexIncrementAsync();
		}

		private void BtnHelpMe_Click(object sender, RoutedEventArgs e)
		{
			HelperStart();
			TxtAnwer.Focus();
		}

		#endregion

		#region methods

		private void PrintCurrentTest()
		{
			if (_testSet.Count < 4)
			{
				throw new Exception("you must add words for test");
			}

			HelperOff();
			TxtAnwer.Clear();
			TxtAnwer.Focus();
			var currentTest = _testSet[_testIndex];
			LabelTestWord.Content = " " + currentTest.Word;

			var example = currentTest.Example;

			example = example.AsteriskReplace(currentTest.TrueAnswer);

			if (string.IsNullOrEmpty(example))
			{
				LabelExample.Visibility = Visibility.Hidden;
				LabelExampleHeader.Visibility = Visibility.Hidden;
			}
			else
			{
				LabelExample.Visibility = Visibility.Visible;
				LabelExampleHeader.Visibility = Visibility.Visible;

				var textBlock = new TextBlock
				{
					Text = example,
					TextWrapping = TextWrapping.Wrap
				};

				LabelExample.Content = textBlock;
			}
		}

		private async Task TestIndexIncrementAsync()
		{
			if (_testIndex < (_testCount - 1))
			{
				_testIndex++;
				PrintCurrentTest();
			}
			else
			{
				await TestResultAsync();
			}
		}

		private async Task CheckAnswerAsync()
		{
			var answer = TxtAnwer.Text;
			var currentTest = _testSet[_testIndex];

			if (answer.Equals(_testSet[_testIndex].TrueAnswer, StringComparison.OrdinalIgnoreCase))
			{
				await SetLevelAsync(true, currentTest.EnRuWordId);
			}
			else
			{
				await SetLevelAsync(false, currentTest.EnRuWordId);
				MessageBox.Show("Fail! answer = " + currentTest.TrueAnswer);
				_failedCount++;
			}
		}

		private async Task SetLevelAsync(bool isTrueAnswer, int wordId)
		{
			var testType = WordLevelManager.TestType.SpellingTest;
			if (_isHelpOn)
			{
				testType = WordLevelManager.TestType.SpellingWithHelpTest;
			}

			var response = await TestsDataProvider.UpdateWordLevel(isTrueAnswer, wordId, testType);

			if (response.IsError)
			{
				throw new Exception(response.ErrorMessage);
			}
		}

		private async Task TestResultAsync()
		{
			var result = string.Format("End of test!\n{0} error from {1} tests",
				_failedCount, _testCount);
			MessageBox.Show(result);
			await RestartTestAsync();
		}

		private async Task RestartTestAsync()
		{
			var startNewTest = DialogHelper.YesNoQuestionDialog(
				"Start new test", "Restart");
			if (startNewTest)
			{
				await StartTestAsync();
			}
			else
			{
				Switcher.Switch(new MainPage());
			}
		}

		private void HelperStart()
		{
			_isHelpOn = true;
			BtnHelpMe.Visibility = Visibility.Hidden;
			LabelHelp.Visibility = Visibility.Visible;
			LabelLetters.Visibility = Visibility.Visible;
			LabelLetters.Content = GetLetters();
		}

		private void HelperOff()
		{
			_isHelpOn = false;
			BtnHelpMe.Visibility = Visibility.Visible;
			LabelHelp.Visibility = Visibility.Hidden;
			LabelLetters.Visibility = Visibility.Hidden;
			LabelLetters.Content = string.Empty;
		}

		private string GetLetters()
		{
			var enWord = _testSet[_testIndex].TrueAnswer;
			var random = new Random();

			var letters = new string(enWord.ToCharArray().
				OrderBy(r => (random.Next(2)%2) == 0).ToArray());
			return letters;
		}

		#endregion

	}
}
