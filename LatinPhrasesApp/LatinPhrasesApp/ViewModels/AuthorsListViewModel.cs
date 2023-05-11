using LatinPhrasesApp.Models;
using LatinPhrasesApp.Services;
using LatinPhrasesApp.Views;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LatinPhrasesApp.ViewModels
{
    public class AuthorsListViewModel : BaseViewModel
    {
        
        private string _searchText;
        private FavoriteLatinPhrasesViewModel _favoriteViewModel;
        public ICommand AddFavoriteCommand { get; set; }

        private ObservableCollection<LatinPhrase> _authors;

        public ObservableCollection<LatinPhrase> Authors
        {
            get => _authors;
            set => SetProperty(ref _authors, value);
        }
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                SearchAuthors(value);
            }
        }
        private LatinPhrase _selectedAuthor;
        private IEnumerable<LatinPhrase> _allAuthors;
        public AuthorsListViewModel()
        {
            // Initialize the Authors property
            
            LoadAuthors();
            AddFavoriteCommand = new Xamarin.Forms.Command<LatinPhrase>(AddFavorite);
            _favoriteViewModel = new FavoriteLatinPhrasesViewModel();
        }
        private void AddFavorite(LatinPhrase phrase)
        {
            if (!_favoriteViewModel.FavoritePhrases.Any(p => p.Latin == phrase.Latin && p.Estonian == phrase.Estonian))
            {
                _favoriteViewModel.FavoritePhrases.Add(phrase);
            }
        }
        private void LoadAuthors()
        {

            _allAuthors = new ObservableCollection<LatinPhrase>
        {
            new LatinPhrase
            {
                Name = "Horace",
                Portrait = "horace_portrait.jpg",
                Latin = "Carpe diem"
            },
            new LatinPhrase
        {
            Name = "Julius Caesar",
            Portrait = "julius_caesar_portrait.jpg",
            Latin = "Veni, vidi, vici"
        },
        new LatinPhrase
        {
            Name = "Virgil",
            Portrait = "virgil_portrait.jpg",
            Latin = "Audentes fortuna iuvat"
        },
        new LatinPhrase
        {
            Name = "Ovid",
            Portrait = "ovid_portrait.jpg",
             Latin = "Tempus edax rerum"
        },
        new LatinPhrase
        {
            Name = "Seneca",
            Portrait = "seneca_portrait.jpg",
             Latin = "Errare humanum est, perseverare diabolicum"
        },
        new LatinPhrase
        {
            Name = "Cicero",
            Portrait = "cicero_portrait.jpg",
             Latin = "Summum ius, summa iniuria"
        },
          new LatinPhrase
        {
            Name = "René Descartes",
            Portrait = "rene_portrait.jpg",
             Latin = "Cogito, ergo sum"
        },
          new LatinPhrase
        {
            Name = "Parmenides",
            Portrait = "parmenides_portrait.jpg",
             Latin = "Ex nihilo nihil fit "
        },
          new LatinPhrase
        {
            Name = "Charles Darwin",
            Portrait = "charles_portrait.jpg",
             Latin = "Natura non facit saltus"
        },
          new LatinPhrase
        {
            Name = "José Bonifácio de Andrada e Silva",
            Portrait = "jose_portrait.jpg",
             Latin = "Non ducor, duco"
        },
           new LatinPhrase
        {
            Name = "Thomas à Kempis",
            Portrait = "thomas_portrait.jpg",
             Latin = "Sic transit gloria mundi "
        },
           new LatinPhrase
        {
            Name = "Aristotle",
            Portrait = "aristotle_portrait.jpg",
             Latin = "Sine qua non"
        },
             new LatinPhrase
        {
            Name = " John Locke",
            Portrait = "john_portrait.jpg",
             Latin = "Tabula rasa"
        },
               new LatinPhrase
        {
            Name = "Publius Syrus",
            Portrait = "publius_portrait.jpg",
             Latin = "Ubi Concordia, Ibi Victoria"
        },
        };
            Authors = new ObservableCollection<LatinPhrase>(_allAuthors);

            OnPropertyChanged(nameof(Authors));
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
       
        public void SearchAuthors(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                Authors = new ObservableCollection<LatinPhrase>(_allAuthors);
            }
            else
            {
                searchText = searchText.ToLowerInvariant();
                var filteredAuthors = _allAuthors.Where(a => a.Name.ToLowerInvariant().Contains(searchText));
                Authors = new ObservableCollection<LatinPhrase>(filteredAuthors);
            }
        }
        public LatinPhrase SelectedAuthor
        {
            get => _selectedAuthor;
            set => SetProperty(ref _selectedAuthor, value);
        }

        public Command LoadAuthorsCommand { get; }
        public Command SearchCommand { get; }
        public Command AuthorTappedCommand { get; }

        
        public void FilterAuthors(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                Authors = new ObservableCollection<LatinPhrase>(_allAuthors);
            }
            else
            {
                Authors = new ObservableCollection<LatinPhrase>(_allAuthors.Where(author => author.Name.ToLower().Contains(searchTerm.ToLower())));
            }
        }

       

        private void OnAuthorTapped(Author author)
        {
            if (author == null)
                return;

            SelectedAuthor = null;
        }

     
    }
}
