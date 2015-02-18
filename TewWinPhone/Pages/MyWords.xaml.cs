using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TewWinPhone.Entities;
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
    public sealed partial class MyWords : Page
    {
        private readonly NavigationService _navigationService = ApplicationContext.NavigationService;
        public bool IsShuffleEnabled;
        public bool IsRepeatEnabled;

        public MyWords()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            RefreshListView();
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

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            IsShuffleEnabled = false;
            IsRepeatEnabled = false;
        }

        private void DeleteWord(object sender, RoutedEventArgs e)
        {
            var word = GetWordFromRoutedEventArgs(e);
            word = ApplicationContext.DbRepository.DeleteWord(word);

            if (word != null)
            {
                new SynchronizeHelper().SendWordInBackGround(word, ApplicationContext.UserEmail);
                RefreshListView();
            }
        }

        private void CleanDb(object sender, RoutedEventArgs e)
        {
            var words = ApplicationContext.DbRepository.GetEnRuWords();
            foreach(var word in words)
            {
                ApplicationContext.DbRepository.DeleteWordFromDb(word);
            }

            RefreshListView();
        }

        private EnglishRussianWordEntity GetWordFromRoutedEventArgs(RoutedEventArgs e)
        {
            var word = (EnglishRussianWordEntity)(((MenuFlyoutItem)e.OriginalSource).DataContext);
            return word;
        }

        private void RefreshListView()
        {
            var words = ApplicationContext.DbRepository.GetEnRuWords(r => r.IsDeleted == false).OrderByDescending(r => r.WordLevel).ToList();
            myWordsListView.ItemsSource = words;
            textBlock.Text = string.Format("My words: {0}", words.Count);
        }

    }
}
