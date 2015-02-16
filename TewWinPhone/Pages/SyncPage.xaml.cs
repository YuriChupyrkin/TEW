using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using TewWinPhone.Entities;
using TewWinPhone.Models;
using TewWinPhone.Services;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TewWinPhone.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SyncPage : Page
    {
        private readonly NavigationService _navigationService = ApplicationContext.NavigationService;
        private CancellationTokenSource _cancellationToken;
        private readonly SynchronizeHelper _syncHelper = new SynchronizeHelper();

        public SyncPage()
        {
            this.InitializeComponent();
            _cancellationToken = new CancellationTokenSource();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //This should be written here rather than the contructor
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            _navigationService.GoBack();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            //remove the handler before you leave!
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        private async void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            //Task.Run(() => StartSync(_cancellationToken), _cancellationToken.Token);
            await StartSync(_cancellationToken);
        }

        private async Task StartSync(CancellationTokenSource cancellationTokenSource)
        {
            var words = ApplicationContext.DbRepository.GetEnRuWords().ToList();

            var updateModel = new UserUpdateDateModel
            {
                UserName = ApplicationContext.UserEmail,
                UpdateDate = DateTime.MinValue.Ticks
            };

            if (words == null || words.Count() == 0)
            {
                //ChangeLabelContent("Step 3. Updating of client ...");
                //updateModel.UpdateDate = lastUpdateDate.Ticks;
                await GetWordsFromServer(updateModel, cancellationTokenSource);
            }

            var lastUpdateDate = words.Max(r => r.UpdateDate);

            var result = await _syncHelper.GetUpdates(ApplicationContext.UserEmail);

            if (result.IsError.GetValueOrDefault())
            {
                await ShowDialog(result.ErrorMessage);
                return;
            }

            if (result.LastUpdate < lastUpdateDate)
            {
                //ChangeLabelContent("Step 3. Updating of cloud ...");
                await UpdateCloud(result.LastUpdate, words, cancellationTokenSource);
            }
            else if (result.LastUpdate > lastUpdateDate)
            {
                //ChangeLabelContent("Step 3. Updating of client ...");
                updateModel.UpdateDate = lastUpdateDate.Ticks;
                await GetWordsFromServer(updateModel, cancellationTokenSource);
                return;
            }
        }

        private async Task GetWordsFromServer(UserUpdateDateModel updateModel, CancellationTokenSource cancellationTokenSource)
        {
            try {
                var userWords = await _syncHelper.GetUserWords(updateModel);

                if (userWords.IsError)
                {
                    await ShowDialog(userWords.ErrorMessage);
                    return;
                }

                AddWordsFromResponse(userWords.WordsCloudModel, cancellationTokenSource);
            }catch(Exception ex)
            {
                await ShowDialog(ex.Message);
            }
        }

        private void AddWordsFromResponse(SyncWordsModel cloudModel, CancellationTokenSource cancellationTokenSource)
        {
            var repositoryFactory = ApplicationContext.DbRepository;
            var user = ApplicationContext.UserEmail;

            if (string.IsNullOrEmpty(user))
            {
                throw new Exception("Words for user " + cloudModel.UserName + ", but user not found");
            }

            var wordCount = cloudModel.Words.Count;

            if (wordCount != 0)
            {
                var count = 0;

                foreach (var word in cloudModel.Words)
                {
                    var enRuWord = new EnglishRussianWordEntity
                    {
                        English = word.English,
                        ExampleOfUse = word.Example,
                        Russian = word.Russian,
                        UpdateDate = word.UpdateDate,
                        WordLevel = word.Level,
                        IsDeleted = word.IsDeleted
                    };
                    repositoryFactory.AddWord(enRuWord);

                    count++;

                    var message = string.Format("Step 4. Adding of words... {0}/{1}", count, wordCount);
                    txtBlockProgress.Text = message;
                    //ChangeLabelContent(message);

                    //var progress = GetProgress(wordCount, count, 100, 0);
                    //ChangeProgressBarValue(progress);


                    CheckCancel(cancellationTokenSource);
                }
            }
            else
            {
                //ChangeProgressBarValue(100);
            }
        }

        private async Task UpdateCloud(DateTime serverLastUpdate, List<EnglishRussianWordEntity> myWords, CancellationTokenSource cancellationTokenSource)
        {
            var updateWords = myWords.Where(r => r.UpdateDate > serverLastUpdate).ToList();

            var packSize = 50;

            var wordCount = updateWords.Count;
            var iterationCount = (wordCount / packSize) + 1;

            if (iterationCount <= 1)
            {
                await CreatWordJsonModelAndSend(updateWords, ApplicationContext.UserEmail);
            }
            else
            {
                var skipCount = 0;
                for (var i = 1; i <= iterationCount + 1; i++)
                {
                    var takeCount = wordCount - (skipCount * packSize) > packSize ? packSize : wordCount;

                    var pack = updateWords.Skip(skipCount * packSize).Take(takeCount);

                    await CreatWordJsonModelAndSend(pack, ApplicationContext.UserEmail);
                    //var progress = GetProgress(iterationCount, i, 100, 0);
                    //ChangeProgressBarValue(progress);
                    skipCount++;

                    CheckCancel(cancellationTokenSource);
                }
            }
        }

        private async Task<ResponseModel> CreatWordJsonModelAndSend(IEnumerable<EnglishRussianWordEntity> enRuWords, string userName)
        {
            var cloudModel = new SyncWordsModel { UserName = userName };

            foreach (var word in enRuWords)
            {
                var viewModel = new WordModel
                {
                    English = word.English,
                    Russian = word.Russian,
                    Level = word.WordLevel,
                    Example = word.ExampleOfUse,
                    IsDeleted = word.IsDeleted,
                    UpdateDate = word.UpdateDate
                };

                cloudModel.Words.Add(viewModel);
            }

            var result = await _syncHelper.SendRequest(cloudModel);

            return result;
        }

        private void CheckCancel(CancellationTokenSource cancellationTokenSource)
        {
            if (cancellationTokenSource.IsCancellationRequested)
            {
                cancellationTokenSource.Token.ThrowIfCancellationRequested();
            }
        }

        private async Task ShowDialog(string message)
        {
            MessageDialog msgbox = new MessageDialog(message);
            await msgbox.ShowAsync();
        }
    }
}
