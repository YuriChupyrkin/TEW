using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EnglishLearnBLL.Models;
using EnglishLearnBLL.Tests;
using EnglishLearnBLL.WordLevelManager;
using WpfUI.Helpers;
using WpfUI.Services;

namespace WpfUI.Pages
{
  /// <summary>
  /// Interaction logic for PickerTest.xaml
  /// </summary>
  public partial class PickerTest : Page
  {
    //private readonly GoogleTranslater _googleTranslater;
    private TestCreator _testCreator;
    private List<PickerTestModel> _testSet;
    private int _testIndex;
    private int _testCount;
    private int _failedCount;
    private const string EnRuTest = "EnRu";
    private const string RuEnTest = "RuEn";
    private string _currentTestName;
    private bool _helpUsed;

    public PickerTest()
    {
      InitializeComponent();

      ApplicationValidator.ExpectAuthorized();
     // _googleTranslater = new GoogleTranslater();

      LabelExampleOfUseLabel.Visibility = Visibility.Hidden;
      BtnDelete.IsEnabled = false;
      BtnHelp.IsEnabled = false;
    }

    #region events

    private void BtnHelp_Click(object sender, RoutedEventArgs e)
    {
      if (_helpUsed)
      {
        return;
      }

      _helpUsed = true;
      var id = _testSet[_testIndex].AnswerId == 0 ? 1 : 0;
      ListTestAnswers.Items.Remove(ListTestAnswers.Items[id]);
    }

    private async void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
      var enWord = _testSet[_testIndex].Word;

      if (_currentTestName == RuEnTest)
      {
				enWord = _testSet[_testIndex].Answers[_testSet[_testIndex].AnswerId];
			}

			await WordsDataProvider.DeleteWordAsync(ApplicationContext.CurrentUser, enWord);
      await TestIndexIncrement();
    }

    private async void BtnEnRuTest_Click(object sender, RoutedEventArgs e)
    {
      _currentTestName = EnRuTest;
      if (_testIndex == 0)
      {
        await StartEnRuTestAsync();
      }
      else
      {
        await RestartTest();
      }
    }

    private async void BtnRuEnTest_Click(object sender, RoutedEventArgs e)
    {
      _currentTestName = RuEnTest;
      if (_testIndex == 0)
      {
        await StartRuEnTestAsync();
      }
      else
      {
        await RestartTest();
      }
    }

    private async void ListTestAnswers_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      var item = ItemsControl
        .ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;

      if (item == null)
      {
        return;
      }

			await CheckAnswerAsync(item.Content.ToString());
      await TestIndexIncrement();
    }

    #endregion

    #region methods

    private async Task StartEnRuTestAsync()
    {
			_testSet = await TestsDataProvider.GetPickerTestModel(
				ApplicationContext.CurrentUser.Id,
				EnglishLearnBLL.WordLevelManager.WordLevelManager.TestType.EnRuTest.ToString());

      StartTest();
      BtnDelete.IsEnabled = true;
      BtnHelp.IsEnabled = true;
    }

    private async Task StartRuEnTestAsync()
    {
			_testSet = await TestsDataProvider.GetPickerTestModel(
				ApplicationContext.CurrentUser.Id,
				EnglishLearnBLL.WordLevelManager.WordLevelManager.TestType.RuEnTest.ToString());

			StartTest();
      BtnDelete.IsEnabled = true;
      BtnHelp.IsEnabled = true;
    }

    private void StartTest()
    {
      if (_testSet.Count < 4)
      {
        throw new Exception("you must add words for test");
      }

      ListTestAnswers.IsEnabled = true;
      _testIndex = 0;
      _testCount = _testSet.Count;
      _failedCount = 0;
      LabelExampleOfUseLabel.Visibility = Visibility.Visible;
      PrintCurrentTest();
    }

    private void PrintCurrentTest()
    {
      _helpUsed = false;
      var test = _testSet[_testIndex];

      LabelTestWord.Content = test.Word;
      ListTestAnswers.Items.Clear();
      foreach (var answer in test.Answers)
      {
        ListTestAnswers.Items.Add(answer);
      }

      var example = test.Example;
      if (_currentTestName == RuEnTest)
      {
        var replacingValue = string.Format("[{0}]", test.Word);
        var replacedValue = test.Answers[test.AnswerId];
        if (example.Contains(replacedValue))
        {
          example = example.Replace(replacedValue, replacingValue);
        }
      }

      var textBlock = new TextBlock
      {
        Text = example,
        TextWrapping = TextWrapping.Wrap
      };

      LabelExample.Content = textBlock;
    }

    private async Task CheckAnswerAsync(string answer)
    {
      var currentTest = _testSet[_testIndex];
      var trueAnswer = currentTest.Answers[currentTest.AnswerId];

      if (answer.Equals(trueAnswer))
      {
				await SetLevelAsync(true, currentTest.WordId);
      }
      else
      {
				await SetLevelAsync(false, currentTest.WordId);
        MessageBox.Show("Fail! answer = " + trueAnswer);
        _failedCount++;
      }
    }

    private async Task SetLevelAsync(bool isTrueAnswer, int wordId)
    {
      var testType = WordLevelManager.TestType.EnRuTest;

      if (_currentTestName.Equals(RuEnTest))
      {
        testType = WordLevelManager.TestType.RuEnTest;
      }

	    var response = await TestsDataProvider.UpdateWordLevel(isTrueAnswer, wordId, testType);

	    if (response.IsError)
	    {
		    throw new Exception(response.ErrorMessage);
	    }
    }

    private async Task TestIndexIncrement()
    {
      if (_testIndex < (_testCount - 1))
      {
        _testIndex++;
        PrintCurrentTest();
      }
      else
      {
        await TestResult();
      }
    }

    private async Task TestResult()
    {
      ListTestAnswers.IsEnabled = false;
      var result = string.Format("End of test!\n{0} error from {1} tests",
        _failedCount, _testCount);
      MessageBox.Show(result);
      await RestartTest();
    }

    private async Task RestartTest()
    {
      var startNewTest = DialogHelper.YesNoQuestionDialog(
        "Start new test", "Restart");
      if (startNewTest)
      {
        if (_currentTestName.Equals(RuEnTest))
        {
          await StartRuEnTestAsync();
        }
        else
        {
					await StartEnRuTestAsync();
        }
      }
      else
      {
        Switcher.Switch(new MainPage());
      }
    }

    #endregion
  }
}
