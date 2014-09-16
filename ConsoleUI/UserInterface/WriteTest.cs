using System;
using System.Linq;
using System.Text;
using ConsoleUI.App;
using ConsoleUI.Helpers;
using EnglishLearnBLL.Tests;

namespace ConsoleUI.UserInterface
{
  internal class WriteTest
  {
    public void StartTest()
    {
      if (!ApplicationValidator.IsAuthorizedUser())
      {
        ConsoleWriteHelper.AccessDenied();
      }

      ConsoleWriteHelper.Clear();
      ConsoleWriteHelper.WriteHeader("English - Russian test");

      var testCreator = new TestCreator(ApplicationContext.RepositoryFactory);
      var testSet = testCreator.WriteTest(ApplicationContext.CurrentUser.Id);

      var testSetCount = testSet.Count();
      var failCount = 0;

      foreach (var writeTestModel in testSet)
      {
        writeTestModel.IsAnswered = false;

        var builder = new StringBuilder();
        builder.AppendFormat("Enter english word").AppendLine();
        builder.AppendFormat("Word: {0}", writeTestModel.Word).AppendLine();
        ConsoleWriteHelper.WriteLine(builder.ToString());

        var answer = ConsoleWriteHelper.ReadLineWithValidationLen(1, 150);

        if (answer.Equals(writeTestModel.TrueAnswer, StringComparison.OrdinalIgnoreCase))
        {
          writeTestModel.IsAnswered = true;
          ConsoleWriteHelper.WriteLine("Ok!");
        }
        else
        {
          ConsoleWriteHelper.RedMessage("Error! Answer = " + writeTestModel.TrueAnswer);
          failCount++;
        }
        testCreator.SetWordLevelForWriteTest(writeTestModel);
        ConsoleWriteHelper.WriteLine("Press any key");
        Console.ReadLine();
        Console.WriteLine("---------------------------------");
      }
      ConsoleWriteHelper.WriteLine(failCount + " errors from " + testSetCount + " tests");
      ConsoleWriteHelper.WriteLine("end of test! Press any key");
      Console.ReadLine();
      ConsoleWriteHelper.StartMenu();
    }
  }
}
