using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Domain.RepositoryFactories;
using EnglishLearnBLL.Models;
using EnglishLearnBLL.Tests;
using WpfUI.Helpers;

namespace WpfUI.Pages
{
  /// <summary>
  /// Interaction logic for PickerTest.xaml
  /// </summary>
  public partial class PickerTest : Page
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private TestCreator _testCreator;
    private List<PickerTestModel> _testSet;
    private int _testIndex = 0;
    private int _testCount = 0;
    private int _failedCount = 0;
    private const string EnRuTest = "EnRu";
    private const string RuEnTest = "RuEn";
    private string _currentTestName;

    public PickerTest()
    {
      InitializeComponent();

      ApplicationValidator.ExpectAuthorized();
      _repositoryFactory = ApplicationContext.RepositoryFactory;
    }

    #region events

    private void BtnEnRuTest_Click(object sender, RoutedEventArgs e)
    {
      _currentTestName = EnRuTest;
      if (_testIndex == 0)
      {
        StartEnRuTest();
      }
      else
      {
        RestartTest();
      }
    }

    private void BtnRuEnTest_Click(object sender, RoutedEventArgs e)
    {
      _currentTestName = RuEnTest;
      if (_testIndex == 0)
      {
        StartRuEnTest();
      }
      else
      {
        RestartTest();
      }
    }

    private void ListTestAnswers_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      var item = ItemsControl
        .ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;

      if (item == null)
      {
        throw new Exception("Answer item is null...");
      }

      CheckAnswer(item.Content.ToString());
      TestIndexIncrement();
    }

    #endregion

    #region methods

    private void StartEnRuTest()
    {
      _testCreator = new TestCreator(ApplicationContext.RepositoryFactory);
      _testSet = _testCreator.EnglishRussianTest(ApplicationContext.CurrentUser.Id).ToList();
      StartTest();
    }

    private void StartRuEnTest()
    {
      _testCreator = new TestCreator(ApplicationContext.RepositoryFactory);
      _testSet = _testCreator.RussianEnglishTest(ApplicationContext.CurrentUser.Id).ToList();
      StartTest();
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
      PrintCurrentTest();
    }

    private void PrintCurrentTest()
    {
      var test = _testSet[_testIndex];

      LabelTestWord.Content = test.Word;
      ListTestAnswers.Items.Clear();
      foreach (var answer in test.Answers)
      {
        ListTestAnswers.Items.Add(answer);
      }
    }
    private void CheckAnswer(string answer)
    {
      var currentTest = _testSet[_testIndex];
      var trueAnswer = currentTest.Answers[currentTest.AnswerId];

      if (answer.Equals(trueAnswer))
      {
        SetLevel(true, currentTest.WordId);
      }
      else
      {
        SetLevel(false, currentTest.WordId);
        MessageBox.Show("Fail! answer = " + trueAnswer);
        _failedCount++;
      }
    }

    private void SetLevel(bool isTrueAnswer, int wordId)
    {
      var levelShift = 1;
      if (_currentTestName.Equals(RuEnTest))
      {
        levelShift = 2;
      }

      levelShift = isTrueAnswer ? levelShift : (0 - levelShift);
      _repositoryFactory.EnRuWordsRepository
          .ChangeWordLevel(wordId, levelShift);
    }

    private void TestIndexIncrement()
    {
      if (_testIndex < (_testCount - 1))
      {
        _testIndex++;
        PrintCurrentTest();
      }
      else
      {
        TestResult();
      }
    }

    private void TestResult()
    {
      ListTestAnswers.IsEnabled = false;
      var result = string.Format("End of test!\n{0} error from {1} tests",
        _failedCount, _testCount);
      MessageBox.Show(result);
      RestartTest();
    }

    private void RestartTest()
    {
      var startNewTest = DialogHelper.YesNoQuestionDialog(
        "Start new test", "Restart");
      if (startNewTest)
      {
        if (_currentTestName.Equals(RuEnTest))
        {
          StartRuEnTest();
        }
        else
        {
          StartEnRuTest();
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
