using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TewWinPhone.Models;
using TewWinPhone.Services;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
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

        public SyncPage()
        {
            this.InitializeComponent();
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
            var words = ApplicationContext.DbRepository.GetEnRuWords();

            var syncWordsModel = new SyncWordsModel
            {
                UserName = ApplicationContext.DbRepository.GetUserEmail()
            };

            foreach(var enRuWords in words)
            {
                var viewModel = new WordModel
                {
                    English = enRuWords.English,
                    Russian = enRuWords.Russian,
                    Level = enRuWords.WordLevel,
                    Example = enRuWords.ExampleOfUse,
                    IsDeleted = enRuWords.IsDeleted,
                    UpdateDate = DateTime.Now
                };

                syncWordsModel.Words.Add(viewModel);
            }

            await new SynchronizeHelper().SendRequest(syncWordsModel);
        }
    }
}
