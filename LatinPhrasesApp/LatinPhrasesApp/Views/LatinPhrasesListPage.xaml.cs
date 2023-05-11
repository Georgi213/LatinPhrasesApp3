using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LatinPhrasesApp.Models;
using LatinPhrasesApp.Services;
using LatinPhrasesApp.ViewModels;
using LatinPhrasesApp.Behaviors;
using LatinPhrasesApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LatinPhrasesApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LatinPhrasesListPage : ContentPage
    {
        private readonly LatinPhrasesListViewModel _viewModel;
        private readonly FavoriteLatinPhrasesViewModel _favoriteViewModel;

        public LatinPhrasesListViewModel LatinPhrasesListViewModel { get; }

        public LatinPhrasesListPage(LatinPhrasesListViewModel viewModel, FavoriteLatinPhrasesViewModel favoriteViewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            AddAboutToolbarItem();

            _favoriteViewModel = favoriteViewModel;
            BindingContext = _viewModel;
        }

        public LatinPhrasesListPage(LatinPhrasesListViewModel latinPhrasesListViewModel)
        {
            LatinPhrasesListViewModel = latinPhrasesListViewModel;
        }

        private async void OnSearchClicked(object sender, EventArgs e)
        {
            var searchText = PhraseSearchBar.Text;
            _viewModel.FilterPhrases(searchText);
        }
        private void HeartButton_Pressed(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                button.TextColor = Color.DarkRed;
            }
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


        private async Task ScrollToSelectedPhrase()
        {
            if (_viewModel.SelectedLatinPhrase != null)
            {
                var selectedPhrase = _viewModel.Phrases.FirstOrDefault(p => p.Latin == _viewModel.SelectedLatinPhrase);

                if (selectedPhrase != null)
                {
                    await Task.Delay(1000);
                    PhrasesListView.ScrollTo(selectedPhrase, position: ScrollToPosition.Center, animated: true);
                }
            }
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ScrollToSelectedPhrase();
        }
        private void HeartButton_Released(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                button.TextColor = Color.Default;
            }
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

        private async void AboutToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPage());
        }
    }
}