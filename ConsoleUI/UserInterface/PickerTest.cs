using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleUI.App;
using ConsoleUI.Helpers;
using EnglishLearnBLL.Models;
using EnglishLearnBLL.Tests;

namespace ConsoleUI.UserInterface
{
  internal class PickerTest
  {
    private const string EnRuTest = "EnRu";
    private const string RuEnTest = "RuEn";

    public void StartEngRusTest()
    {
      if (!ApplicationValidator.IsAuthorizedUser())
      {
        ConsoleWriteHelper.AccessDenied();
      }

      EngRusTest();
    }

    public void StartRusEngTest()
    {
      if (!ApplicationValidator.IsAuthorizedUser())
      {
        ConsoleWriteHelper.AccessDenied();
      }

      RusEngTest();
    }

    private void EngRusTest()
    {
      ConsoleWriteHelper.Clear();
      ConsoleWriteHelper.WriteHeader("English - Russian test");

      var testCreator = new TestCreator(ApplicationContext.RepositoryFactory);
      var testSet = testCreator.EnglishRussianTest(ApplicationContext.CurrentUser.Id);

      WriteTests(testSet, testCreator);
    }

    private void RusEngTest()
    {
      ConsoleWriteHelper.Clear();
      ConsoleWriteHelper.WriteHeader("Russian - English test");

      var testCreator = new TestCreator(ApplicationContext.RepositoryFactory);
      var testSet = testCreator.RussianEnglishTest(ApplicationContext.CurrentUser.Id);

      WriteTests(testSet, testCreator);
    }

    private void WriteTests(IEnumerable<PickerTestModel> testSet, TestCreator testCreator)
    {
      var allTestCount = testSet.Count();
      var failCount = 0;

      foreach (var test in testSet)
      {
        WriteTestMessage(test);
        var isTrueAnswer = PickAnswer(test, testCreator, RuEnTest);

        if (isTrueAnswer == false)
        {
          failCount++;
        }

        ConsoleWriteHelper.WriteLine("Press any key");
        Console.ReadLine();
        Console.WriteLine("-------------------------------------------------");
      }
      ConsoleWriteHelper.WriteLine(failCount + " errors from " + allTestCount + " tests");
      ConsoleWriteHelper.WriteLine("end of test! Press any key");
      Console.ReadLine();
      ConsoleWriteHelper.StartMenu();
    }

    private static void WriteTestMessage(PickerTestModel testModel)
    {
      var message = new StringBuilder();
      message.AppendFormat("Word: {0}\n", testModel.Word);

      var count = 1;
      foreach (var answer in testModel.Answers)
      {
        message.AppendFormat("{0}) {1}\n", count, answer);
        count++;
      }
      ConsoleWriteHelper.WriteOnNewLine(message.ToString());
    }

    private bool PickAnswer(PickerTestModel testModel, TestCreator testCreator, string testName)
    {
      ConsoleWriteHelper.WriteLine("Enter answer");
      bool isAnswerPicked = false;
      var anserId = 0;
      while (!isAnswerPicked)
      {
        try
        {
          anserId = int.Parse(ConsoleWriteHelper.ReadLine());
          if (anserId > 0 && anserId < 5)
          {
            isAnswerPicked = true;
          }
          else
          {
            ConsoleWriteHelper.RedMessage("Incorrect answer! Try again");
          }
        }
        catch
        {
          ConsoleWriteHelper.RedMessage("Incorrect answer! Try again");
        }
      }

      string trueAnswer = string.Empty;
      if (testName == EnRuTest)
      {
        trueAnswer = testCreator.SetAnswerOnEnRuTest(testModel, anserId - 1);
      }
      else if (testName == RuEnTest)
      {
        trueAnswer = testCreator.SetAnswerOnRuEnTest(testModel, anserId - 1);
      }

      if (trueAnswer == testModel.Answers[anserId - 1])
      {
        ConsoleWriteHelper.WriteLine("OK!");
        return true;
      }
      else
      {
        ConsoleWriteHelper.RedMessage("Error!!! Answer = " + trueAnswer);
        return false;
      }
    }
  }
}
