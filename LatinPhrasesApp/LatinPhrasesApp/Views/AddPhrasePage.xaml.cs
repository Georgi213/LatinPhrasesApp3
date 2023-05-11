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
    public partial class AddPhrasePage : ContentPage
    {
        private readonly TaskCompletionSource<LatinPhrase> _taskCompletionSource;
        private MyLatinPhrasesViewModel _viewModel;
        private readonly Action<LatinPhrase> _addPhraseAction;


        public Task<LatinPhrase> GetNewPhraseAsync()
        {
            return _taskCompletionSource.Task;
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            // Get the user input from the Entry controls
            var newPhrase = GetEnteredPhrase();

            // Call the AddPhrase action
            _addPhraseAction(newPhrase);

            // Navigate back to the previous page
            await Navigation.PopModalAsync();
        }
       
        public AddPhrasePage(Action<LatinPhrase> addPhraseAction)
        {
            InitializeComponent();
            _addPhraseAction = addPhraseAction;
            BindingContext = _viewModel;
        }

        private LatinPhrase GetEnteredPhrase()
        {
            string latin = Latin.Text;
            string estonian = Estonian.Text;

            return new LatinPhrase
            {
                Latin = latin,
                Estonian = estonian
            };
        }
    }
}