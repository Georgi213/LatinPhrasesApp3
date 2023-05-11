using LatinPhrasesApp.Models;
using LatinPhrasesApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LatinPhrasesApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using LatinPhrasesApp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LatinPhrasesApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthorsListPage : ContentPage
    {
        
            private AuthorsListViewModel _viewModel;
        private FavoriteLatinPhrasesViewModel _favoriteViewModel;
        public AuthorsListPage()
            {
            InitializeComponent();
            AddAboutToolbarItem();

            BindingContext = _viewModel = new AuthorsListViewModel();
        }

         

            

            private async void OnSearchClicked(object sender, EventArgs e)
            {
            var searchTerm = AuthorSearchBar.Text;
            _viewModel.FilterAuthors(searchTerm);
           }

        private async void OnAuthorSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is LatinPhrase author)
            {
                await _viewModel.NavigateToLatinPhrasesListPage(author);
            }

          
            AuthorsListView.SelectedItem = null;
        }
        public async Task NavigateToLatinPhrasesListPage(LatinPhrase author)
        {
            var latinPhrasesListViewModel = new LatinPhrasesListViewModel(_favoriteViewModel, author.Latin);
            var latinPhrasesListPage = new LatinPhrasesListPage(latinPhrasesListViewModel, _favoriteViewModel);

            
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