﻿using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using EnglishLearnBLL.Models;
using EnglishLearnBLL.View;
using WpfUI.Helpers;

namespace WpfUI.Pages
{
    /// <summary>
    /// Interaction logic for MyWordPage.xaml
    /// </summary>
    public partial class MyWordPage : Page
    {
        private const string TitlePage = "My words";
        public MyWordPage()
        {
            InitializeComponent();
            ApplicationValidator.ExpectAuthorized();
            ViewWords();
        }

        #region events

        private void DataGridWords_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedWord = DataGridWords.SelectedValue as WordViewModel;

            if (selectedWord != null)
            {
                var isDelete = DialogHelper
                  .YesNoQuestionDialog("Delete word \"" + selectedWord.English + "\"?", "Deleting");
                if (isDelete)
                {
                    var repositoryFactory = ApplicationContext.RepositoryFactory;

                    var resultWord = repositoryFactory.EnRuWordsRepository.MakeDeleted(selectedWord.English, ApplicationContext.CurrentUser.Id);
                    var syncHelper = new SynchronizeHelper();
                    syncHelper.SendWordInBackGround(resultWord, ApplicationContext.CurrentUser);
                    ViewWords();
                }
            }
        }

        #endregion

        #region methods

        private void ViewWords()
        {
            DataGridWords.IsReadOnly = true;
            var userId = ApplicationContext.CurrentUser.Id;
            var wordViewer = new WordViewer(ApplicationContext.RepositoryFactory);

            var words = wordViewer.ViewWords(userId)
              .OrderByDescending(r => r.Level);
            LabelMyWords.Content = TitlePage + ": " + words.Count();

            DataGridWords.ItemsSource = words;
        }

        #endregion
    }
}
