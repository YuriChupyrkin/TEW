using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Domain.RepositoryFactories;
using EnglishLearnBLL.Models;
using EnglishLearnBLL.Tests;
using EnglishLearnBLL.WordLevelManager;
using WpfUI.Helpers;

namespace WpfUI.Pages
{
  /// <summary>
  /// Interaction logic for PickerTest.xaml
  /// </summary>
  public partial class PickerTest : Page
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly WordLevelManager _wordLevelManager;
    private readonly GoogleTranslater _googleTranslater;
    private TestCreator _testCreator;
    private List<PickerTestModel> _testSet;
    private int _testIndex;
    private int _testCount;
    private int _failedCount;
    private const string EnRuTest = "EnRu";
    private const string RuEnTest = "RuEn";
    private string _currentTestName;

    public PickerTest()
    {
      InitializeComponent();

      ApplicationValidator.ExpectAuthorized();
      _repositoryFactory = ApplicationContext.RepositoryFactory;
      _wordLevelManager = new WordLevelManager(_repositoryFactory);
      _googleTranslater = new GoogleTranslater();

      LabelExampleOfUseLabel.Visibility = Visibility.Hidden;
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
      LabelExampleOfUseLabel.Visibility = Visibility.Visible;
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
      Speak(test.Word);
    }

    private void Speak(string word)
    {
      if (MainWindow.IsOnlineVersion == false)
      {
        return;
      }
    
      if (_currentTestName == EnRuTest && MainWindow.IsSpeakEng)
      {
        _googleTranslater.Speak(word, "en");
      }
      if (_currentTestName == RuEnTest && MainWindow.IsSpeakRus)
      {
        _googleTranslater.Speak(word, "ru");
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
      var testType = WordLevelManager.TestType.EnRuTest;
      if (_currentTestName.Equals(RuEnTest))
      {
        testType = WordLevelManager.TestType.RuEnTest;
      }
      _wordLevelManager.SetWordLevel(wordId, isTrueAnswer, testType);
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
