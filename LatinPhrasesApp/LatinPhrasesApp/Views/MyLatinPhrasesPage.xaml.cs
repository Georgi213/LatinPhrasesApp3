using LatinPhrasesApp.Models;
using LatinPhrasesApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LatinPhrasesApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyLatinPhrasesPage : ContentPage
    {
        private readonly MyLatinPhrasesViewModel _viewModel;
        public MyLatinPhrasesViewModel MyLatinPhrasesViewModel { get; }

        public MyLatinPhrasesPage(MyLatinPhrasesViewModel viewModel)
        {
            InitializeComponent();
            AddAboutToolbarItem();
            _viewModel = viewModel;

            BindingContext = _viewModel;

            MessagingCenter.Subscribe<AddPhrasePage, LatinPhrase>(this, "AddPhrase", (sender, phrase) =>
            {
                _viewModel.Phrases.Add(phrase);
            });
        }

        
        private void AddAboutToolbarItem()
        {
            var aboutToolbarItem = new ToolbarItem
            {
                Text = "Umbes",
                IconImageSource = "about_icon.png", // Optional, add your about icon image
                Order = ToolbarItemOrder.Secondary, // Set the order for the action overflow menu
                Priority = 0 // Adjust the priority as needed
            };

            aboutToolbarItem.Clicked += AboutToolbarItem_Clicked;
            this.ToolbarItems.Add(aboutToolbarItem);
        }

        private async void OnTapped(object sender, EventArgs e)
        {
            if (sender is StackLayout stackLayout && stackLayout.Parent is Grid grid)
            {
                var shadingBox = grid.Children.OfType<BoxView>().FirstOrDefault();

                if (shadingBox != null)
                {
                    shadingBox.IsVisible = true;
                    await shadingBox.FadeTo(0.3, 100);
                    await Task.Delay(0);
                    await shadingBox.FadeTo(0, 100);
                    shadingBox.IsVisible = false;
                }
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<AddPhrasePage, LatinPhrase>(this, "AddPhrase");
        }
        private async void AboutToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPage());
        }

        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is LatinPhrase tappedPhrase)
            {
                var action = await DisplayActionSheet("Valige toiming", "Tühista", null, "Muuda", "Kustuta");

                switch (action)
                {
                    case "Muuda":
                        _viewModel.EditPhrase(tappedPhrase);
                        break;
                    case "Kustuta":
                        _viewModel.DeletePhrase(tappedPhrase);
                        break;
                }
            }
        }
        private async void OnSearchClicked(object sender, EventArgs e)
        {
            var searchTerm = PhraseSearchBar.Text;
            _viewModel.FilterPhrases(searchTerm);
        }
        private async void OnRefreshing(object sender, EventArgs e)
        {
            await _viewModel.LoadPhrases();
            PhrasesListView.IsRefreshing = false;
        }
        private void OnSearchTextChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SearchBar.Text))
            {
                (BindingContext as MyLatinPhrasesViewModel).FilterPhrases((sender as SearchBar).Text);
            }
        }

    }
}