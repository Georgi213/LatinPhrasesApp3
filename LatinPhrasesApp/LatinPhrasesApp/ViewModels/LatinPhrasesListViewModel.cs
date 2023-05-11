using LatinPhrasesApp.Models;
using LatinPhrasesApp.Services;
using LatinPhrasesApp.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LatinPhrasesApp.ViewModels
{
    public class LatinPhrasesListViewModel : BaseViewModel
    {
        
         
        private FavoriteLatinPhrasesViewModel _favoriteViewModel;
        private ObservableCollection<LatinPhrase> _allPhrases;
        public ICommand AddFavoriteCommand { get; set; }
        public ICommand ShareCommand { get; }
        private string _searchText;
        public ICommand CopyPhraseCommand { get; }
        private ObservableCollection<LatinPhrase> _phrases;
        private LatinPhrasesListViewModel _viewModel;
        public string SelectedLatinPhrase { get; set; }

        public ObservableCollection<LatinPhrase> Phrases
        {
            get => _phrases;
            set => SetProperty(ref _phrases, value);
        }
        private void FilterPhrasesByLegendaryQuote(string legendaryQuote)
        {
            if (!string.IsNullOrEmpty(legendaryQuote))
            {
                var filteredPhrases = _allPhrases.Where(p => p.Latin == legendaryQuote);
                Phrases = new ObservableCollection<LatinPhrase>(filteredPhrases);
            }
            else
            {
                Phrases = new ObservableCollection<LatinPhrase>(_allPhrases);
            }
        }
        public async Task NavigateToLatinPhrasesListPage(LatinPhrase author)
        {
            var latinPhrasesListViewModel = new LatinPhrasesListViewModel(_favoriteViewModel, author.Latin);
            var latinPhrasesListPage = new LatinPhrasesListPage(latinPhrasesListViewModel, _favoriteViewModel);

            if (Application.Current.MainPage is FlyoutPage flyoutPage &&
            flyoutPage.Detail is NavigationPage navigationPage)
            {
                await navigationPage.Navigation.PushAsync(latinPhrasesListPage);
            }
        }
        public void CopyPhraseToClipboard(string phrase)
        {
            Xamarin.Essentials.Clipboard.SetTextAsync(phrase);
        }

        public LatinPhrasesListViewModel(FavoriteLatinPhrasesViewModel favoriteViewModel, string selectedPhrase = null)
        {
            _favoriteViewModel = favoriteViewModel;
            AddFavoriteCommand = new Xamarin.Forms.Command<LatinPhrase>(AddFavorite);
            CopyPhraseCommand = new Xamarin.Forms.Command<string>(CopyPhraseToClipboard);
            ShareCommand = new Xamarin.Forms.Command<LatinPhrase>(SharePhrase);



            LoadPhrases(selectedPhrase);

            MessagingCenter.Subscribe<AuthorsListViewModel, string>(this, "AuthorSelected", (sender, legendaryQuote) =>
            {
                FilterPhrasesByLegendaryQuote(legendaryQuote);
            });

        }
        private async void SharePhrase(LatinPhrase phrase)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = $"{phrase.Latin} - {phrase.Estonian}",
                Title = "Share Latin Phrase"
            });
        }
        private void AddFavorite(LatinPhrase phrase)
        {
            if (!_favoriteViewModel.FavoritePhrases.Any(p => p.Latin == phrase.Latin && p.Estonian == phrase.Estonian))
            {
                _favoriteViewModel.FavoritePhrases.Add(phrase);
            }
        }
        private void LoadPhrases(string selectedPhrase = null)
        {
           
            _allPhrases = new ObservableCollection<LatinPhrase>
        {
            new LatinPhrase { Latin = "Carpe diem",     Estonian = "Haara päevast"},
             new LatinPhrase { Latin = "Veni, vidi, vici", Estonian = "Tulin, nägin, võitsin" },
                 new LatinPhrase { Latin = "Alea iacta est", Estonian = "Täring on veeretatud" },
                 new LatinPhrase { Latin = "Calamitas virtutis occasio", Estonian = "Katastroof on võimalus vooruseks." },
                 new LatinPhrase { Latin = "Dant gaudea vires", Estonian = "Rõõmus annab jõudu" },
                 new LatinPhrase { Latin = "Fabricando fit faber", Estonian = "Meister on loodud tööjõuga" },
              new LatinPhrase { Latin = "Jactantius maerent, qui minus dolent", Estonian = "Kurbus, mis näitab vähest kurbust" },
               new LatinPhrase { Latin = "Rebus in adversis meliora sperare memento", Estonian = "Ebaõnnestumises looda parimat" },
                 new LatinPhrase { Latin = "Tamdiu discendum est, quamdiu vivas", Estonian = "Kui palju sa elad, nii palju sa õpid" },
                 new LatinPhrase { Latin = "Beate vivere est honeste vivere", Estonian = "Elada õnnelikult tähendab elada ilusti" },
                 new LatinPhrase { Latin = "Ubi Concordia, Ibi Victoria", Estonian = "Kus on kokkulepe, seal on võit." },
                 new LatinPhrase { Latin = "Vae victoribus", Estonian = "Häda võitjatele" },
                  new LatinPhrase { Latin = "Sine qua non", Estonian = "Ilma milleta mitte" },
                 new LatinPhrase { Latin = "Laus propria sordet", Estonian = "Kiitus teie kasuks on rõve" },
                 new LatinPhrase { Latin = "Tempus edax rerum", Estonian = "Aja õgimine" },
                 new LatinPhrase { Latin = "In legibus salus", Estonian = "Pääste seaduses" },
                 new LatinPhrase { Latin = "Errando discimus", Estonian = "Vead õpetavad" },
        };
            
            if (!string.IsNullOrEmpty(selectedPhrase))
            {
                Phrases = new ObservableCollection<LatinPhrase>(_allPhrases.Where(p => p.Latin == selectedPhrase));
            }
            else
            {
                Phrases = new ObservableCollection<LatinPhrase>(_allPhrases);
            }



            OnPropertyChanged(nameof(Phrases));
        }
       

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void SearchPhrases(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                Phrases = new ObservableCollection<LatinPhrase>(_allPhrases);
            }
            else
            {
                searchText = searchText.ToLowerInvariant();
                var filteredPhrases = _allPhrases.Where(a => a.Latin.ToLowerInvariant().Contains(searchText));
                Phrases = new ObservableCollection<LatinPhrase>(filteredPhrases);
            }
        }

        public void FilterPhrases(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                Phrases = new ObservableCollection<LatinPhrase>(_allPhrases);
            }
            else
            {
                Phrases = new ObservableCollection<LatinPhrase>(_allPhrases.Where(phrase => phrase.Latin.ToLower().Contains(searchText.ToLower())));
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                SearchPhrases(value);
            }
        }
    }

    
}
