using System.Windows;

namespace WpfUI.Helpers
{
  internal class DialogHelper
  {
    public static bool YesNoQuestionDialog(string message, string caption)
    {
      const MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
      const MessageBoxImage icnMessageBox = MessageBoxImage.Question;

      MessageBoxResult rsltMessageBox = MessageBox.Show(message, caption, btnMessageBox, icnMessageBox);

      switch (rsltMessageBox)
      {
        case MessageBoxResult.Yes:
          return true;
        case MessageBoxResult.No:
          return false;
      }
      return true;
    }

  }
}
