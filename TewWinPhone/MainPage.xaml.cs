using TewWinPhone.Pages;
using TewWinPhone.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace TewWinPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly NavigationService _navigationService = ApplicationContext.NavigationService;
        private readonly DbRepository _dbRepository = ApplicationContext.DbRepository;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            //todo if not email
            _navigationService.Navigate(typeof(ChangeEmailPage));
        }

        private void BtnAddWord_Click(object sender, RoutedEventArgs e)
        {
            if (_dbRepository.IsUserHaveEmail() == false)
            {
                _navigationService.Navigate(typeof(ChangeEmailPage));
            }
            else
            {
                _navigationService.Navigate(typeof(AddWordPage));
            }
        }

        private void myWords_Click(object sender, RoutedEventArgs e)
        {
            if (_dbRepository.IsUserHaveEmail() == false)
            {
                _navigationService.Navigate(typeof(ChangeEmailPage));
            }
            else
            {
                _navigationService.Navigate(typeof(MyWords));
            }
        }

        private void btnTests_Click(object sender, RoutedEventArgs e)
        {
            if (_dbRepository.IsUserHaveEmail() == false)
            {
                _navigationService.Navigate(typeof(ChangeEmailPage));
            }
            else
            {
                _navigationService.Navigate(typeof(ChooseTestPage));
            }
        }

        private void btnSync_Click(object sender, RoutedEventArgs e)
        {
            if (_dbRepository.IsUserHaveEmail() == false)
            {
                _navigationService.Navigate(typeof(ChangeEmailPage));
            }
            else
            {
                _navigationService.Navigate(typeof(SyncPage));
            }
        }

        private void btnEmail_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.Navigate(typeof(ChangeEmailPage));
        }

      
    }
}
