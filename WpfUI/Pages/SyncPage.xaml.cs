using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Domain.Entities;
using EnglishLearnBLL.Models;
using WpfUI.Helpers;

namespace WpfUI.Pages
{
  /// <summary>
  /// Interaction logic for SyncPage.xaml
  /// </summary>
  public partial class SyncPage : Page
  {
    private CancellationTokenSource _cancellationToken;
    private SynchronizeHelper _syncHelper;
    private User _user;
    private ResponseModel _responseModel;

    public SyncPage()
    {
      InitializeComponent();
    }

    private void Grid_Loaded(object sender, RoutedEventArgs e)
    {
      _cancellationToken = new CancellationTokenSource();

      try
      {
        Task.Run(() => StartSync(_cancellationToken), _cancellationToken.Token);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
      _cancellationToken.Cancel();
    }

    #region privates

    private void StartSync(CancellationTokenSource cancellationTokenSource)
    {
      try
      {
        Sync(cancellationTokenSource);
      }
      catch (Exception ex)
      {
        CloseSync(ex.Message);
      }
    }

    private void Sync(CancellationTokenSource cancellationTokenSource)
    {
      _user = ApplicationContext.CurrentUser;

      if (_user == null)
      {
        CloseSync("Sign in please");
      }

      ChangeLabelContent("Step 1. User Ok...");

      var repositoryFactory = ApplicationContext.RepositoryFactory;
      _syncHelper = new SynchronizeHelper(repositoryFactory);

      var isServerOnline = _syncHelper.IsServerOnline();

      if (isServerOnline == false)
      {
        CloseSync("Server is offline");
      }

      CheckCancel(cancellationTokenSource);

      ChangeLabelContent("Step 2. Server is online. Synchronizing...");   
      SendMyWords();
      ChangeProgressBarValue(30);
      CheckCancel(cancellationTokenSource);

      ChangeLabelContent("Step 3. Wait for response...");
      GetUserWords();
      ChangeProgressBarValue(35);
      CheckCancel(cancellationTokenSource);

      ChangeLabelContent("Step 4. Response complete...");
      var responseModel = _responseModel.WordsCloudModel;

      if (responseModel == null)
      {
        CloseSync("response model is null");
      }

      ChangeProgressBarValue(40);
      CheckCancel(cancellationTokenSource);

      ChangeLabelContent("Step 5. Adding words...");  
      AddWordsFromResponse(responseModel, cancellationTokenSource);
      CheckCancel(cancellationTokenSource);

      ChangeLabelContent("Step 6. Removing old word...");
      RemoveIsDeletedWords(_user.Id, cancellationTokenSource);
      CheckCancel(cancellationTokenSource);

      ChangeLabelContent("Complete");
      ChangeProgressBarValue(100);
    
      CloseSync("Success");
    }

    private void SendMyWords()
    {
      _responseModel = _syncHelper.SendMyWords(_user);
      if (_responseModel.IsError)
      {
        CloseSync(_responseModel.ErrorMessage);
      }
    }

    private void GetUserWords()
    {
      _responseModel = _syncHelper.GetUserWords(_user);
      if (_responseModel.IsError)
      {
        CloseSync(_responseModel.ErrorMessage);
      }
    }

    private void CheckCancel(CancellationTokenSource cancellationTokenSource)
    {
      if (cancellationTokenSource.IsCancellationRequested)
      {
        cancellationTokenSource.Token.ThrowIfCancellationRequested();
      }
    }

    private void AddWordsFromResponse(WordsCloudModel cloudModel, CancellationTokenSource cancellationTokenSource)
    {
      var repositoryFactory = ApplicationContext.RepositoryFactory;
      var user = repositoryFactory.UserRepository
        .Find(r => r.Email == cloudModel.UserName);

      if (user == null)
      {
        throw new Exception("Words for user " + cloudModel.UserName + ", but user not found");
      }

      var wordCount = cloudModel.Words.Count;

      if (wordCount != 0)
      {
        var count = 0;

        foreach (var word in cloudModel.Words)
        {
          repositoryFactory.EnRuWordsRepository
            .AddTranslate(
              word.English,
              word.Russian,
              word.Example,
              user.Id,
              word.UpdateDate,
              word.Level);

          count++;

          var message = string.Format("Step 5. Adding words... {0}/{1}", count, wordCount);
          ChangeLabelContent(message);

          var progress = GetProgress(wordCount, count, 55, 40);
          ChangeProgressBarValue(progress);

          CheckCancel(cancellationTokenSource);
        }
      }
      else
      {
        ChangeProgressBarValue(95);
      }
    }

    private void RemoveIsDeletedWords(int userId, CancellationTokenSource cancellationTokenSource)
    {
      var repositoryFactory = ApplicationContext.RepositoryFactory;
      var deletedWords = repositoryFactory.EnRuWordsRepository.AllEnRuWords().Where(r => r.IsDeleted && r.UserId == userId);
  
      var count = 0;

      var wordCount = deletedWords.Count();

      if (wordCount != 0)
      {
        foreach (var word in deletedWords)
        {
          repositoryFactory.EnRuWordsRepository.DeleteEnRuWord(word.EnglishWord.EnWord, userId);

          count++;
          var message = string.Format("Step 6. Removing old word... {0}/{1}", count, wordCount);
          ChangeLabelContent(message);


          var progress = GetProgress(wordCount, count, 5, 95);
          ChangeProgressBarValue(progress);

          CheckCancel(cancellationTokenSource);
        }
      }
      else
      {
        ChangeProgressBarValue(99);
      }
    }

    private int GetProgress(double totalWords, double currentWord, double totalProgress, int startProgress)
    {
      var progress = (int)((totalProgress / totalWords) * currentWord);
      progress += startProgress;
      return progress;
    }

    private void ChangeLabelContent(string content)
    {
      Dispatcher.BeginInvoke(DispatcherPriority.Background, new ThreadStart(() => { LabelSync.Content = content; }));
    }

    private void ChangeProgressBarValue(int value)
    {
      Dispatcher.BeginInvoke(DispatcherPriority.Background, new ThreadStart(() => { ProgressBarSync.Value = value; }));
    }

    private void CloseSync(string message = "")
    {
      if (string.IsNullOrEmpty(message))
      {
        MessageBox.Show(message, "Synchronize");
      }
      Dispatcher.BeginInvoke(DispatcherPriority.Background, new ThreadStart(() => Switcher.Switch(new MainPage())));
    }

    #endregion
  }
}
