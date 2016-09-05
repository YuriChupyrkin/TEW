﻿using System;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using Common.Mail;
using WpfUI.Autofac;
using WpfUI.Helpers;
using WpfUI.Pages;
using WpfUI.Services;

namespace WpfUI
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private const string Version = "(2.00   9/4/2016)"; 
    public const string AppName = "TEW";
    public static MainWindow ThisWindow { get; set; }
	  public static Frame WindowMainFrame { get; private set; }

    public MainWindow()
    {
      InitializeComponent();
      ResizeMode = ResizeMode.CanMinimize;

			Title = string.Format("{0} {1}", AppName, Version);

      StartApp();
      
      Dispatcher.UnhandledException += (sender, args) =>
      {      
        args.Handled = true;
        Switcher.Switch(new ErrorPage(args.Exception.Message));
      };

	    WindowMainFrame = MainFrame;
    }

    #region events

    private void MenuItem_Click_1(object sender, RoutedEventArgs e)
    {
      Environment.Exit(1);
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
			MainFrame.IsEnabled = true;
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
                             "year: 2014 - 2016\n";
      MessageBox.Show(message, "About");
    }

    private void HowTo_Click(object sender, RoutedEventArgs e)
    {
      var messageBuilder = new StringBuilder();
      messageBuilder.AppendLine("Add word: 'Menu' -> 'Add words'.");
      messageBuilder.AppendLine("Update word: 'Menu' -> 'Add words' -> enter new translate.");
      messageBuilder.AppendLine("Delete word: 'Menu' -> 'My words' -> double click.");

      messageBuilder.AppendLine();
      messageBuilder.AppendLine("Tests:");
      messageBuilder.AppendLine("English-Russian translate test. Max level of words: 5");
      messageBuilder.AppendLine("Russian-English translate test. Max level of words: 10");
      messageBuilder.AppendLine("Spelling test with help. Max level of words: 15");
      messageBuilder.AppendLine("Spelling test without help. Max level of words: 20");

      MessageBox.Show(messageBuilder.ToString(), "How to");
    }

    private void ResetPassword_Click(object sender, RoutedEventArgs e)
    {
      Switcher.Switch(new RestorePasswordPage());
    }

    private void ChangePassword_Click(object sender, RoutedEventArgs e)
    {
      Switcher.Switch(new ChangePasswordPage());
    }

    private async void ResetWordsLevelMenu_Click(object sender, RoutedEventArgs e)
    {
      if (DialogHelper.YesNoQuestionDialog("Reset word level?", "Reset"))
      {
        ApplicationValidator.ExpectAuthorized();
	      await TestsDataProvider.ResetWordsLevel(ApplicationContext.CurrentUser);
      }
    }

    private void ExportMenu_Click(object sender, RoutedEventArgs e)
    {
			MessageBox.Show("This function is not available");
			return;

      if (ApplicationContext.CurrentUser == null)
      {
        MessageBox.Show("Sign in please");
        return;
      }

      // Create OpenFileDialog 
      var saveDialog = new Microsoft.Win32.SaveFileDialog();

      // Set filter for file extension and default file extension 
      saveDialog.DefaultExt = ".xml";
      saveDialog.Filter = "XML Files (*.xml)|*.xml";

      bool? result = saveDialog.ShowDialog();

      if(result == true)
      {
        var fileName = saveDialog.FileName;
        //var wordsExporter = new WordsExporter(ApplicationContext.RepositoryFactory);
				// wordsExporter.Export(ApplicationContext.CurrentUser.Id, fileName);
      }
    }

    private void ImportMenu_Click(object sender, RoutedEventArgs e)
    {
			MessageBox.Show("This function is not available");
			return;

      if (ApplicationContext.CurrentUser == null)
      {
        MessageBox.Show("Sign in please");
        return;
      }

      // Create OpenFileDialog 
      var openDialog = new Microsoft.Win32.OpenFileDialog();

      // Set filter for file extension and default file extension 
      openDialog.DefaultExt = ".xml";
      openDialog.Filter = "XML Files (*.xml)|*.xml";

      // Display OpenFileDialog by calling ShowDialog method 
      bool? result = openDialog.ShowDialog();

      // Get the selected file name and display in a TextBox 
      if (result == true)
      {
        // Open document 
        var filename = openDialog.FileName;
        //var wordsImporter = new WordsImporter(ApplicationContext.RepositoryFactory);
        //wordsImporter.Import(ApplicationContext.CurrentUser.Email, filename);
      }
    }

    private void TewCloudMenu_Click(object sender, RoutedEventArgs e)
    {
      var startInfo = new ProcessStartInfo("explorer.exe", "http://tew.azurewebsites.net/");
      Process.Start(startInfo);
    }

    #endregion

    #region methods

    private void StartApp()
    {
      ThisWindow = this;
      var container = AutofacModule.RegisterAutoFac();
      ApplicationContext.AutoFacContainer = container;
      ApplicationContext.EmailSender = container.BeginLifetimeScope().Resolve<IEmailSender>();

      Switcher.PageSwitcher = this;

      try
      {
        Switcher.Switch(new SignIn());
      }
      catch (Exception ex)
      {
        MessageBox.Show("Global error! " + ex.Message);
        Switcher.Switch(new ErrorPage(ex.Message));
				MainWindow.WindowMainFrame.IsEnabled = true;
			}
    }

    public void Navigate(Page nextPage)
    {
      MainFrame.Navigate(nextPage);
    }

    #endregion
  }
}
