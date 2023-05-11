using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LatinPhrasesApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LatinPhrasesApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LettersPage : ContentPage
    {
        public LettersPage()
        {
            InitializeComponent();
        }

        private async void OnLetterTapped(object sender, EventArgs e)
        {
            var letterLabel = sender as Label;
            var letter = letterLabel.Text;
            var letterPhrasesViewModel = new LetterPhrasesViewModel();
            await letterPhrasesViewModel.LoadPhrases();
            letterPhrasesViewModel.FilterPhrasesByLetter(letter);

            var letterPhrasesPage = new LetterPhrasesPage(letterPhrasesViewModel.Phrases)
            {
                BindingContext = letterPhrasesViewModel
            };

            await Navigation.PushAsync(letterPhrasesPage);
        }
    }
}