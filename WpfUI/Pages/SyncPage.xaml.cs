using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Domain.Entities;
using EnglishLearnBLL.Models;
using WpfUI.Helpers;
using Domain.RepositoryFactories;

namespace WpfUI.Pages
{
  /// <summary>
  /// Interaction logic for SyncPage.xaml
  /// </summary>
  public partial class SyncPage : Page
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private CancellationTokenSource _cancellationToken;
    private SynchronizeHelper _syncHelper;
    private User _user;
    private readonly SynchronizeHelper _synchronizeHelper;

    public SyncPage()
    {
      InitializeComponent();
      _repositoryFactory = ApplicationContext.RepositoryFactory;
      _synchronizeHelper = new SynchronizeHelper();
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
      CheckServer();
      CheckCancel(cancellationTokenSource);

      CheckUpdates(cancellationTokenSource);

      RemoveAllDeletedWords();

      CloseSync("Success");
    }

    private void RemoveAllDeletedWords()
    {
      _repositoryFactory.EnRuWordsRepository.RemoveAllDeletedWords(ApplicationContext.CurrentUser.Id);
    }

    private void CheckUpdates(CancellationTokenSource cancellationTokenSource)
    {
      ChangeLabelContent("Step 2. Get update information ...");
      var user = ApplicationContext.CurrentUser;

      var words = _repositoryFactory.EnRuWordsRepository.AllEnRuWords().Where(r => r.UserId == user.Id).ToList();

      WordsCloudModel cloudModel = null;

      var updateModel = new UserUpdateDateModel
      {
        UserName = user.Email,
        UpdateDate = DateTime.MinValue.Ticks
      };

      if (words == null || words.Any() == false)
      {
        ChangeLabelContent("Step 3. Updating of client ...");
        cloudModel = GetWordsFromServer(updateModel, cancellationTokenSource);
      }
      else
      {
        var lastUpdate = words.Max(r => r.UpdateDate);

        var result = _synchronizeHelper.GetUpdates(user.Email);

        if (result.IsError.GetValueOrDefault())
        {
          MessageBox.Show(result.ErrorMessage, "sync error");
          return;
        }

        if (result.LastUpdate < lastUpdate)
        {
          ChangeLabelContent("Step 3. Updating of cloud ...");
          cloudModel = UpdateCloud(result.LastUpdate, words, cancellationTokenSource);
        }
        else if (result.LastUpdate > lastUpdate)
        {
          ChangeLabelContent("Step 3. Updating of client ...");
          updateModel.UpdateDate = lastUpdate.Ticks;
          cloudModel = GetWordsFromServer(updateModel, cancellationTokenSource);
        }
      }

      var localTotalWords = GetWordCount(user.Email);
      if (cloudModel != null && localTotalWords != cloudModel.TotalWords)
      {
        if (localTotalWords > cloudModel.TotalWords)
        {
          //todo updateCloud
          ChangeLabelContent("Force updating of cloud ...");
          words = _repositoryFactory.EnRuWordsRepository.AllEnRuWords().Where(r => r.UserId == user.Id).ToList();
          UpdateCloud(DateTime.MinValue, words, cancellationTokenSource);
        }
        else
        {
          //todo updateClient
          ChangeLabelContent("Force updating of client ...");
          updateModel.UpdateDate = 0;
          GetWordsFromServer(updateModel, cancellationTokenSource);
        }
      }
    }

    private int GetWordCount(string userName)
    {
      return _repositoryFactory.EnRuWordsRepository.AllEnRuWords().Count(r => r.User.Email == userName);
    }

    private WordsCloudModel UpdateCloud(DateTime serverLastUpdate, List<EnRuWord> myWords,
      CancellationTokenSource cancellationTokenSource)
    {
      var updateWords = myWords.Where(r => r.UpdateDate > serverLastUpdate).ToList();

      var packSize = 50;

      var wordCount = updateWords.Count;
      var iterationCount = (wordCount/packSize) + 1;

      ResponseModel responseModel = null;

      if (iterationCount <= 1)
      {
        responseModel = CreatWordJsonModelAndSend(updateWords, ApplicationContext.CurrentUser);
      }
      else
      {
        var skipCount = 0;
        for (var i = 1; i <= iterationCount + 1; i++)
        {
          var takeCount = wordCount - (skipCount*packSize) > packSize ? packSize : wordCount;

          var pack = updateWords.Skip(skipCount*packSize).Take(takeCount);

          responseModel = CreatWordJsonModelAndSend(pack, ApplicationContext.CurrentUser);
          var progress = GetProgress(iterationCount, i, 100, 0);
          ChangeProgressBarValue(progress);
          skipCount++;

          CheckCancel(cancellationTokenSource);
        }
      }

      return responseModel.WordsCloudModel;
    }

    private WordsCloudModel GetWordsFromServer(UserUpdateDateModel updateModel,
      CancellationTokenSource cancellationTokenSource)
    {
      var userWords = _synchronizeHelper.GetUserWords(updateModel);

      if (userWords.IsError)
      {
        MessageBox.Show(userWords.ErrorMessage);
        return null;
      }

      AddWordsFromResponse(userWords.WordsCloudModel, cancellationTokenSource);

      return userWords.WordsCloudModel;
    }

    private void CheckServer()
    {
      _syncHelper = new SynchronizeHelper();

      var isServerOnline = _syncHelper.IsServerOnline();

      if (isServerOnline == false)
      {
        CloseSync("Server is offline");
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
              word.Level,
              word.IsDeleted);

          count++;

          var message = string.Format("Step 4. Adding of words... {0}/{1}", count, wordCount);
          ChangeLabelContent(message);

          var progress = GetProgress(wordCount, count, 100, 0);
          ChangeProgressBarValue(progress);

          CheckCancel(cancellationTokenSource);
        }
      }
      else
      {
        ChangeProgressBarValue(100);
      }
    }

    private int GetProgress(double totalWords, double currentWord, double totalProgress, int startProgress)
    {
      var progress = (int) ((totalProgress/totalWords)*currentWord);
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
      if (string.IsNullOrEmpty(message) == false)
      {
        MessageBox.Show(message, "Synchronize");
      }
      Dispatcher.BeginInvoke(DispatcherPriority.Background, new ThreadStart(() => Switcher.Switch(new MainPage())));
    }

    private ResponseModel CreatWordJsonModelAndSend(IEnumerable<EnRuWord> enRuWords, User user)
    {
      var cloudModel = new WordsCloudModel {UserName = user.Email};

      foreach (var word in enRuWords)
      {
        var viewModel = new WordJsonModel
        {
          English = word.EnglishWord.EnWord,
          Russian = word.RussianWord.RuWord,
          Level = word.WordLevel,
          Example = word.Example,
          IsDeleted = word.IsDeleted,
          UpdateDate = word.UpdateDate
        };

        cloudModel.Words.Add(viewModel);
      }

      var result = _syncHelper.SendRequest(cloudModel);

      return result;
    }

    #endregion
  }
}
