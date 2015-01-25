﻿using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TewWinPhone.Entities;
using TewWinPhone.Services;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TewWinPhone.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddWordPage : Page
    {
        private readonly NavigationService _navigationService = ApplicationContext.NavigationService;

        public AddWordPage()
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

        private async void txtBoxEnglish_LostFocus(object sender, RoutedEventArgs e)
        {
            var googleTranslater = new GoogleTranslater();
            var translate = await googleTranslater.GetTranslate(txtBoxEnglish.Text);

            ClearTranslatesAndExamples();
            foreach (var t in translate.Translates)
            {
                listBoxTranslate.Items.Add(t);
            }

            txtBoxExample.Text = translate.Example;
        }

        private void listBoxTranslate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = e.AddedItems.FirstOrDefault();
            if (item != null)
            {
                var word = item.ToString();
                txtBoxRussian.Text = word;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        { 
            if(txtBoxEnglish.Text.Length < 1 || txtBoxRussian.Text.Length < 1)
            {
                return;
            }

            var word = new EnglishRussianWordEntity
            {
                English = txtBoxEnglish.Text,
                Russian = txtBoxRussian.Text,
                ExampleOfUse = txtBoxExample.Text
            };

            ApplicationContext.DbRepository.AddWord(word);
            ClearAllInputs();
        }
      
        private void ClearTranslatesAndExamples()
        {
            listBoxTranslate.Items.Clear();
            txtBoxExample.Text = string.Empty;
        }

        private void ClearAllInputs()
        {
            txtBoxEnglish.Text = string.Empty;
            txtBoxRussian.Text = string.Empty;
            txtBoxExample.Text = string.Empty;
            ClearTranslatesAndExamples();
        }

    }
}