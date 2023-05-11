using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LatinPhrasesApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : FlyoutPage
    {
        public MainPage()
        {
            InitializeComponent();
            Flyout = new MenuPage();
            Detail = NavigationPageInstance;

        }
        private async void OnAboutClicked(object sender, EventArgs e)
        {
            await NavigationPageInstance.Navigation.PushAsync(new AboutPage());
        }

        

    }
}