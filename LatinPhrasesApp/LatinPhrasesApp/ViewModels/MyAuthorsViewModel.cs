using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using LatinPhrasesApp.Models;
using LatinPhrasesApp.Views;
using MvvmHelpers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using MvvmHelpers.Commands;
using Xamarin.Forms;
using System.Linq;
using Command = Xamarin.Forms.Command;

namespace LatinPhrasesApp.ViewModels
{
    public class MyAuthorsViewModel : BaseViewModel
    {
        public ObservableCollection<LatinPhrase> Phrases
        {
            get => _authors;
            set => SetProperty(ref _authors, value);
        }
        public ICommand AddAuthorCommand { get; set; }
        private ObservableCollection<LatinPhrase> _authors;
        private IEnumerable<LatinPhrase> _allAuthors;

        private ImageSource _portrait;
        public ImageSource Portrait
        {
            get { return _portrait; }
            set
            {
                _portrait = value;
                OnPropertyChanged(nameof(Portrait)); 
            }
        }
        private string _searchText;
        public ICommand EditAuthorCommand { get; set; }
        public ICommand FilterAuthorsCommand { get; set; }
        public ICommand CopyPhraseCommand { get; }
        private readonly MyLatinPhrasesPage _page;
        public ICommand ShareCommand { get; }
        private IEnumerable<LatinPhrase> _allPhrases;
        public ICommand DeletePhraseCommand { get; set; }
        private MyLatinPhrasesViewModel _viewModel;
        public MyAuthorsViewModel()
        {
            LoadAuthors();
            FilterAuthorsCommand = new Xamarin.Forms.Command<string>(FilterAuthors);
            AddAuthorCommand = new Command(async () =>
            {
                var addAuthorPage = new AddAuthorPage(AddAuthor);
                DeletePhraseCommand = new Xamarin.Forms.Command<LatinPhrase>(DeleteAuthor);

                var addAuthorNavigationPage = new NavigationPage(addAuthorPage);
                await Application.Current.MainPage.Navigation.PushModalAsync(addAuthorNavigationPage);
            });
            ShareCommand = new Xamarin.Forms.Command<LatinPhrase>(SharePhrase);
            EditAuthorCommand = new Xamarin.Forms.Command<LatinPhrase>(async (author) => await EditAuthor(author));
        }
        private void SaveAuthors()
        {
            var json = JsonConvert.SerializeObject(Phrases);
            Preferences.Set("Authors", json);
        }
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    FilterAuthors(_searchText);
                }
            }
        }
        public void SearchAuthors(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                Phrases = new ObservableCollection<LatinPhrase>(_allAuthors);
            }
            else
            {
                searchText = searchText.ToLowerInvariant();
                var filteredAuthors = _allAuthors.Where(a => a.Name.ToLowerInvariant().Contains(searchText));
                Phrases = new ObservableCollection<LatinPhrase>(filteredAuthors);
            }
        }
        public void CopyPhraseToClipboard(string author)
        {
            Xamarin.Essentials.Clipboard.SetTextAsync(author);
        }
        private ObservableCollection<LatinPhrase> LoadAuthorsFromStorage()
        {

            var json = Preferences.Get("Authors", string.Empty);
            if (!string.IsNullOrEmpty(json))
            {
                return JsonConvert.DeserializeObject<ObservableCollection<LatinPhrase>>(json);
            }
            return null;
        }


        public void UpdateAuthor(LatinPhrase author)
        {
            var index = Phrases.IndexOf(author);
            if (index != -1)
            {
                Phrases[index] = author;
                SaveAuthors();
            }
        }
        public async void DeleteAuthor(LatinPhrase phrases)
        {
            bool confirmDelete = await Application.Current.MainPage.DisplayAlert("Kustuta autor", "Kas olete kindel, et soovite selle autori kustutada?", "Jah", "ei");

            if (confirmDelete)
            {

                Phrases.Remove(phrases);
                SaveAuthors();
            }
        }
        public void FilterAuthors(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                Phrases = new ObservableCollection<LatinPhrase>(_allAuthors);
            }
            else
            {
                Phrases = new ObservableCollection<LatinPhrase>(_allAuthors.Where(author => author.Name.ToLower().Contains(searchTerm.ToLower())));
            }
        }
        public async Task LoadAuthors()
        {
            _allAuthors = LoadAuthorsFromStorage();
            if (_allAuthors == null || _allAuthors.Count() == 0)
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



        };
            }


            Phrases = new ObservableCollection<LatinPhrase>(_allAuthors);
            OnPropertyChanged(nameof(Phrases));
        }
        public void AddAuthor(LatinPhrase newPhrase)
        {

            Phrases.Add(newPhrase);
            SaveAuthors();
        }
        private async void SharePhrase(LatinPhrase author)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = $"{author.Name} - {author.Latin}",
                Title = "Share Latin Author "
            });
        }

        public async Task EditAuthor(LatinPhrase author)
        {
            var editAuthorPage = new EditAuthorPage(author, this);
            await Application.Current.MainPage.Navigation.PushModalAsync(editAuthorPage);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
