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
    public partial class LetterPhrasesPage : ContentPage
    {
        public LetterPhrasesPage(IEnumerable<LatinPhrase> phrases)
        {
            InitializeComponent();

            BindingContext = new LetterPhrasesViewModel(phrases);
        }
    }
}