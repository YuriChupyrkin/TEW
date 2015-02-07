using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class ChangeEmailPage : Page
    {
        private readonly NavigationService _navigationService = ApplicationContext.NavigationService;

        public ChangeEmailPage()
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
            //This should be written here rather than the contructor
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            var currentEmail = ApplicationContext.DbRepository.GetUserEmail();
            textBlockCurrentEmail.Text = currentEmail;
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

        private async void btnOk_Click(object sender, RoutedEventArgs e)
        {
            var email = txtBoxEmail.Text;

            if (ApplicationValidator.IsValidatEmail(email) == false)
            {
                await ShowDialog("Incorrect email");
            }

            else
            {
                ApplicationContext.DbRepository.SetUserEmail(email);
                ApplicationContext.UserEmail = email;
                _navigationService.GoBack();
            }
        }

        private async Task ShowDialog(string message)
        {
            MessageDialog msgbox = new MessageDialog(message);
            await msgbox.ShowAsync();
        }
    }
}
