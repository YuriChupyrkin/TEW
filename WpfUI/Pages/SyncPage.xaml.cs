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
            CloseSync("Success");
        }

        private void CheckUpdates(CancellationTokenSource cancellationTokenSource)
        {
            ChangeLabelContent("Step 2. Get update information ...");
            var user = ApplicationContext.CurrentUser;

            var words = _repositoryFactory.EnRuWordsRepository.AllEnRuWords().Where(r => r.UserId == user.Id).ToList();

            var updateModel = new UserUpdateDateModel
            {
                UserName = user.Email,
                UpdateDate = DateTime.MinValue.Ticks
            };

            if (words == null || words.Any() == false)
            {
                ChangeLabelContent("Step 3. Updating of client ...");
                GetWordsFromServer(updateModel, cancellationTokenSource);
                return;
            }

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
                UpdateCloud(result.LastUpdate, words);
            }
            else if (result.LastUpdate > lastUpdate)
            {
                ChangeLabelContent("Step 3. Updating of client ...");
                updateModel.UpdateDate = lastUpdate.Ticks;
                GetWordsFromServer(updateModel, cancellationTokenSource);
                return;
            }
        }

        private void UpdateCloud(DateTime serverLastUpdate, List<EnRuWord> myWords)
        {
            var updateWords = myWords.Where(r => r.UpdateDate > serverLastUpdate).ToList();
            CreatWordJsonModelAndSend(updateWords, ApplicationContext.CurrentUser);
        }

        private void GetWordsFromServer(UserUpdateDateModel updateModel, CancellationTokenSource cancellationTokenSource)
        {
            var userWords = _synchronizeHelper.GetUserWords(updateModel);

            if (userWords.IsError)
            {
                MessageBox.Show(userWords.ErrorMessage);
                return;
            }

            AddWordsFromResponse(userWords.WordsCloudModel, cancellationTokenSource);
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
                        word.Level);

                    count++;

                    var message = string.Format("Step 4. Adding of words... {0}/{1}", count, wordCount);
                    ChangeLabelContent(message);

                    var progress = GetProgress(wordCount, count, 40, 60);
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
            if (string.IsNullOrEmpty(message) == false)
            {
                MessageBox.Show(message, "Synchronize");
            }
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new ThreadStart(() => Switcher.Switch(new MainPage())));
        }

        private ResponseModel CreatWordJsonModelAndSend(IEnumerable<EnRuWord> enRuWords, User user)
        {
            var cloudModel = new WordsCloudModel { UserName = user.Email };

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
