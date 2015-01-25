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
        private ResponseModel _responseModel;


        public SyncPage()
        {
            InitializeComponent();
            _repositoryFactory = ApplicationContext.RepositoryFactory;
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

            //CheckServer
            ChangeLabelContent("Step 1. User Ok...");
            CheckServer();
            CheckCancel(cancellationTokenSource);

            ////Send words
            //ChangeLabelContent("Step 2. Server is online. Sync...");
            //SendMyWords(cancellationTokenSource, _user);
            //CheckCancel(cancellationTokenSource);

            ////wait response
            //ChangeLabelContent("Step 3. Waiting for response...");
            //GetUserWords();
            //ChangeProgressBarValue(55);
            //CheckCancel(cancellationTokenSource);

            ////waiting...
            //ChangeLabelContent("Step 4. Wait please...");
            //var responseModel = _responseModel.WordsCloudModel;

            //if (responseModel == null)
            //{
            //    CloseSync("response model is null");
            //}

            //ChangeProgressBarValue(60);
            //CheckCancel(cancellationTokenSource);

            ////Adding of words
            //ChangeLabelContent("Step 5. Adding of words...");
            //AddWordsFromResponse(responseModel, cancellationTokenSource);
            //CheckCancel(cancellationTokenSource);

            ////Removing of words
            //ChangeLabelContent("Step 6. Removing of words...");
            //RemoveIsDeletedWords(_user.Id, cancellationTokenSource);
            //CheckCancel(cancellationTokenSource);

            ////Complete
            //ChangeLabelContent("Complete");
            //ChangeProgressBarValue(100);

            CheckUpdates();
            CloseSync("Success");
        }

        private void CheckUpdates()
        {
            ChangeLabelContent("Step 2. Get update information ...");
            var user = ApplicationContext.CurrentUser;

            var words = _repositoryFactory.EnRuWordsRepository.AllEnRuWords().Where(r => r.Id == user.Id).ToList();

            if(words == null || words.Any() == false)
            {
                MessageBox.Show("need update CLIENT");
                return;
            }

            var lastUpdate = words.Max(r => r.UpdateDate);
            MessageBox.Show(lastUpdate.ToShortTimeString(), "LOCAL DATE");

            var result = new SynchronizeHelper().GetUpdates(user.Email);

            if (result.IsError.GetValueOrDefault())
            {
                MessageBox.Show(result.ErrorMessage, "sync error");
            }

            MessageBox.Show(result.LastUpdate.ToShortTimeString());

            if (result.LastUpdate == lastUpdate)
            {
                MessageBox.Show("ok");
            }
            else if(result.LastUpdate < lastUpdate)
            {
                MessageBox.Show("need update CLOUD");
            }
            else
            {
                MessageBox.Show("need update CLIENT 2");
            }
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

                    var message = string.Format("Step 5. Adding of words... {0}/{1}", count, wordCount);
                    ChangeLabelContent(message);

                    var progress = GetProgress(wordCount, count, 35, 60);
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
                    var message = string.Format("Step 6. Removing of words... {0}/{1}", count, wordCount);
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
            if (string.IsNullOrEmpty(message) == false)
            {
                MessageBox.Show(message, "Synchronize");
            }
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new ThreadStart(() => Switcher.Switch(new MainPage())));
        }

        private void SendMyWords(CancellationTokenSource cancellationTokenSource, User user)
        {
            var repositoryFactory = ApplicationContext.RepositoryFactory;

            var userWords = repositoryFactory.EnRuWordsRepository
              .AllEnRuWords().Where(r => r.UserId == user.Id);

            if (userWords == null)
            {
                CloseSync("User words is null");
            }

            const int wordsInRequest = 100;

            var iterationCount = 0;
            if (userWords.Count() % wordsInRequest != 0)
            {
                iterationCount = userWords.Count() / wordsInRequest + 1;
            }
            else
            {
                iterationCount = userWords.Count() / wordsInRequest;
            }

            var sendList = userWords.Take(wordsInRequest);
            var result = CreatWordJsonModelAndSend(sendList, user);
            if (result.IsError)
            {
                CloseSync(result.ErrorMessage);
            }

            var progress = GetProgress(iterationCount, 1, 50, 0);
            ChangeProgressBarValue(progress);
            CheckCancel(cancellationTokenSource);

            for (var i = 1; i < iterationCount; i++)
            {
                sendList = userWords.Skip(wordsInRequest * i).Take(wordsInRequest);

                result = CreatWordJsonModelAndSend(sendList, user);
                if (result.IsError)
                {
                    CloseSync(result.ErrorMessage);
                }

                progress = GetProgress(iterationCount, i + 1, 50, 0);
                ChangeProgressBarValue(progress);
                CheckCancel(cancellationTokenSource);
            }
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
