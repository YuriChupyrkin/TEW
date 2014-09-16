using System.Windows.Controls;

namespace WpfUI
{
  internal static class Switcher
  {
    public static MainWindow PageSwitcher;

    public static void Switch(Page newPage)
    {
      PageSwitcher.Navigate(newPage);
    }
  }
}
