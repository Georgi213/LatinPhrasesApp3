using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LatinPhrasesApp.Models;
using LatinPhrasesApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LatinPhrasesApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditPhrasePage : ContentPage
    {
        private LatinPhrase _phrase;
        private MyLatinPhrasesViewModel _viewModel;

        public EditPhrasePage(LatinPhrase phrase, MyLatinPhrasesViewModel viewModel)
        {
            InitializeComponent();

            _phrase = phrase;
            _viewModel = viewModel;

            // Populate Entry controls with the existing Latin and Estonian phrases
            Latin.Text = phrase.Latin;
            Estonian.Text = phrase.Estonian;
        }

        private async void OnSaveButtonClicked(object sender, System.EventArgs e)
        {
            // Get the user input from the Entry controls
            string latin = Latin.Text;
            string estonian = Estonian.Text;

            // Update the phrase object with the new values
            _phrase.Latin = latin;
            _phrase.Estonian = estonian;


            _viewModel.UpdatePhrase(_phrase);
            // Navigate back to the previous page
            await Navigation.PopModalAsync();
        }
    }
}
