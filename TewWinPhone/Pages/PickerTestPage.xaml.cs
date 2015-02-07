using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TewWinPhone.Entities;
using TewWinPhone.Models;
using TewWinPhone.Services;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI;
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
    public sealed partial class PickerTestPage : Page
    {
        private Color _defaultButtonColor = Colors.Gold;

        private readonly NavigationService _navigationService = ApplicationContext.NavigationService;
        private List<PickerTestModel> _testModels;
        private int _currentTestIndex;
        private bool _isPicked;
        private int _errorCount;

        public PickerTestPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            var userWords = ApplicationContext.DbRepository.GetEnRuWords() as List<EnglishRussianWordEntity>;

            if (userWords.Count < 5)
            {
                await ShowDialog("Need more words");
                _navigationService.GoBack();
                return;
            }

            if (ApplicationContext.CurrentPickerTest == PickerTest.EnRu)
            {
                _testModels = new TestCreater().CreateEnglishRussianTest(userWords).ToList();
            }
            else
            {
                _testModels = new TestCreater().CreateRussianEnglishTest(userWords).ToList();
            }

            _errorCount = 0;
            await ViewNextTest(_currentTestIndex);
        }

        #region events

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


        private void btnAnswer1_Click(object sender, RoutedEventArgs e)
        {
            if(_isPicked == false)
                CheckAwnser(0, sender);
        }

        private void btnAnswer2_Click(object sender, RoutedEventArgs e)
        {
            if (_isPicked == false)
                CheckAwnser(1, sender);
        }

        private void btnAnswer3_Click(object sender, RoutedEventArgs e)
        {
            if (_isPicked == false)
                CheckAwnser(2, sender);
        }

        private void btnAnswer4_Click(object sender, RoutedEventArgs e)
        {
            if (_isPicked == false)
                CheckAwnser(3, sender);
        }


        private async void btnNext_Click(object sender, RoutedEventArgs e)
        {
            Picked(false);
            await ViewNextTest(_currentTestIndex);
        }

        #endregion

        #region methods

        private async Task ViewNextTest(int index)
        {
            SetDefaultButtonColor();
            if (index > _testModels.Count - 1)
            {
                var resultMessage = string.Format("{0} error(s)", _errorCount);
                await ShowDialog(resultMessage);
                _navigationService.GoBack();
                return;
            }

            var testModel = _testModels[index];
            txtBlockEnglisWord.Text = testModel.Word;
            txtBlockExample.Text = testModel.Example;

            btnAnswer1.Content = testModel.Answers[0];
            btnAnswer2.Content = testModel.Answers[1];
            btnAnswer3.Content = testModel.Answers[2];
            btnAnswer4.Content = testModel.Answers[3];
        }

        private void CheckAwnser(int answerIndex, object sender)
        {
            if (_currentTestIndex > _testModels.Count - 1)
            {             
                return;
            }

            Picked(true);

            var isTrueAnswer = _testModels[_currentTestIndex].AnswerId == answerIndex;
            var button = sender as Button;

            if (isTrueAnswer)
            {
                button.Background = new SolidColorBrush(Colors.Green);
                UpdateLevel(_testModels[_currentTestIndex].WordId, true);
            }
            else
            {
                _errorCount++;
                button.Background = new SolidColorBrush(Colors.Red);
                ShowTrueAnswer();
                UpdateLevel(_testModels[_currentTestIndex].WordId, false);
            }

            _currentTestIndex++;
        }

        private void Picked(bool isPicked)
        {
            _isPicked = isPicked;
            btnNext.IsEnabled = isPicked;
        }

        private void SetDefaultButtonColor()
        {
            btnAnswer1.Background = new SolidColorBrush(_defaultButtonColor);
            btnAnswer2.Background = new SolidColorBrush(_defaultButtonColor);
            btnAnswer3.Background = new SolidColorBrush(_defaultButtonColor);
            btnAnswer4.Background = new SolidColorBrush(_defaultButtonColor);
        }

        private void ShowTrueAnswer()
        {
            switch (_testModels[_currentTestIndex].AnswerId)
            {
                case 0:
                    btnAnswer1.Background = new SolidColorBrush(Colors.Green);
                    break;
                case 1:
                    btnAnswer2.Background = new SolidColorBrush(Colors.Green);
                    break;
                case 2:
                    btnAnswer3.Background = new SolidColorBrush(Colors.Green);
                    break;
                case 3:
                    btnAnswer4.Background = new SolidColorBrush(Colors.Green);
                    break;
            }
        }

        private async Task ShowDialog(string message)
        {
            MessageDialog msgbox = new MessageDialog(message);
            await msgbox.ShowAsync();
        }

        private void UpdateLevel(int wordId, bool isLevelUp)
        {
            var result = ApplicationContext.DbRepository.UpdateLevel(wordId, isLevelUp, ApplicationContext.CurrentPickerTest);
            new SynchronizeHelper().SendWordInBackGround(result, ApplicationContext.UserEmail);
        }

        #endregion
    }
}
