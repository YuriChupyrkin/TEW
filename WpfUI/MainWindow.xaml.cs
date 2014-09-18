using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using Common.Mail;
using Domain.RepositoryFactories;
using WpfUI.Auth;
using WpfUI.Autofac;
using WpfUI.Helpers;
using WpfUI.Pages;

namespace WpfUI
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public const string AppName = "TEW";

    public static MainWindow ThisWindow { get; set; }
    public static EventHandler<EventArgs> ChangeTitleEvent { get; set; }

    public static bool IsOnlineVersion
    {
      get
      {
        var isConnected = CheckConnection();
        var title = string.Format("{0} (offline version)", AppName);
        if (isConnected)
        {
          title = string.Format("{0} (online version)", AppName);
        }
        ChangeTitleEvent(title, null);
        return isConnected;
      }
    }

    public MainWindow()
    {
      InitializeComponent();
      ResizeMode = ResizeMode.CanMinimize;

      Title = "TEW";

      StartApp();
      
      Dispatcher.UnhandledException += (sender, args) =>
      {      
        args.Handled = true;
        Switcher.Switch(new ErrorPage(args.Exception.Message));
      };
    }

    #region events

    private void MenuItem_Click_1(object sender, RoutedEventArgs e)
    {
      Environment.Exit(1);
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
      if (ApplicationContext.CurrentUser == null)
      {
        Switcher.Switch(new SignIn());
      }
      else
      {
        Switcher.Switch(new MainPage());
      }
    }

    private void SignOut_Click(object sender, RoutedEventArgs e)
    {
      ApplicationContext.CurrentUser = null;
      Switcher.Switch(new SignIn());
    }

    private void About_Click(object sender, RoutedEventArgs e)
    {
      const string message = "TEW (Trainer of English Words)\n" +
                             "author: Yuri Chupyrkin\n" +
                             "email: yuri.chupyrkin@gmail.com\n" +
                             "year: 2014\n";
      MessageBox.Show(message, "About");
    }

    private void HowTo_Click(object sender, RoutedEventArgs e)
    {
      const string message = "Add word: 'Menu' -> 'Add words'.\n" +
                             "Update word: 'Menu' -> 'Add words' -> enter new translate.\n" +
                             "Delete word: 'Menu' -> 'My words' -> double click.\n";
      MessageBox.Show(message, "How to");
    }

    private void ResetPassword_Click(object sender, RoutedEventArgs e)
    {
      Switcher.Switch(new RestorePasswordPage());
    }

    private void ChangePassword_Click(object sender, RoutedEventArgs e)
    {
      Switcher.Switch(new ChangePasswordPage());
    }

    private void MenuConnect_Click(object sender, RoutedEventArgs e)
    {
      if (IsOnlineVersion)
      {
        SetAdminAuthentication();
      }
      else
      {
        MessageBox.Show("Failure to Internet connect", "Connection");
      }
    }

    private void RemoveExcessWordsMenu_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        var repositoryFactory = ApplicationContext.RepositoryFactory;
        var removedCount = repositoryFactory.EnRuWordsRepository.ClearEnWords();
        removedCount += repositoryFactory.EnRuWordsRepository.ClearRuWords();

        MessageBox.Show(string.Format("{0} word(s) was removed", removedCount));
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Words removing error");
      }
    }

    #endregion

    #region methods

    private void ChangeTitleForApp(object sender, EventArgs args)
    {
      var title = (string) sender;
      Dispatcher.Invoke(new Action(() => { Title = title; })); 
    }

    private void StartApp()
    {
      ChangeTitleEvent += ChangeTitleForApp;

      ThisWindow = this;
      var container = AutofacModule.RegisterAutoFac();
      var repositoryFactory = container.BeginLifetimeScope().Resolve<IRepositoryFactory>();
      ApplicationContext.AutoFacContainer = container;
      ApplicationContext.RepositoryFactory = repositoryFactory;
      ApplicationContext.EmailSender = container.BeginLifetimeScope().Resolve<IEmailSender>();
      SetAdminAuthentication();
      ApplicationContext.BingTranslater = new BingTranslater();

      Switcher.PageSwitcher = this;

      try
      {
        Switcher.Switch(new SignIn());
      }
      catch (Exception ex)
      {
        MessageBox.Show("Global error! " + ex.Message);
        Switcher.Switch(new ErrorPage(ex.Message));
      }
    }

    public void Navigate(Page nextPage)
    {
      MainFrame.Navigate(nextPage);
    }

    public static void SetAdminAuthentication()
    {
      var task = new Task(CrateAdminAuthentication);
      task.Start();
    }

    private static void CrateAdminAuthentication()
    {
      if (IsOnlineVersion)
      {
        try
        {
          var adminAuth = new AdminAuthentication(
            "TrainerOfEnglishWords",
            "LeTcA3cONe0r9IlwED1MBZ/5RMTeZsJdmqLpddpmKOg=");

          ApplicationContext.AdminAuthentication = adminAuth;
        }
        catch (Exception ex)
        {
          var message = string.Format("Online translator could not connect..." +
                                      "\nYou will use offline version" +
                                      "\n{0}", ex.Message);
          MessageBox.Show(message, "connection error");
        }
      }
    }

    private static bool CheckConnection()
    {
      var client = new WebClient();
      try
      {
        using (client.OpenRead("http://www.google.com"))
        {
        }
        return true;
      }
      catch (WebException)
      {
        return false;
      }
    }
    #endregion
  }
}
