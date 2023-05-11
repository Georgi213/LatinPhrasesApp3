using LatinPhrasesApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LatinPhrasesApp.Behaviors;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LatinPhrasesApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoriteLatinPhrasesPage : ContentPage
    {
        private readonly FavoriteLatinPhrasesViewModel _viewModel;

        public FavoriteLatinPhrasesPage(FavoriteLatinPhrasesViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
            AddAboutToolbarItem();
        }
        private void HeartButton_Pressed(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                button.TextColor = Color.DarkRed;
            }
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