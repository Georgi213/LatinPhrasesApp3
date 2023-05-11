using LatinPhrasesApp.Models;
using LatinPhrasesApp.Services;
using LatinPhrasesApp.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Command = Xamarin.Forms.Command;
using System.Linq;

namespace LatinPhrasesApp.ViewModels
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class MyLatinPhrasesViewModel : BaseViewModel
    {
        public ObservableCollection<LatinPhrase> Phrases
        {
            get => _phrases;
            set => SetProperty(ref _phrases, value);
        }
        public ICommand AddPhraseCommand { get; set; }
        private IEnumerable<LatinPhrase> _allPhrases;
        private ObservableCollection<LatinPhrase> _phrases;
        private ObservableCollection<LatinPhrase> _authors;

       
        private string _searchText;
        public ICommand EditPhraseCommand { get; set; }
        public ICommand FilterPhrasesCommand { get; set; }
        public ICommand CopyPhraseCommand { get; }
        private readonly MyLatinPhrasesPage _page;
        public ICommand ShareCommand { get; }
        public ICommand DeletePhraseCommand { get; set; }
        private MyLatinPhrasesViewModel _viewModel;
        public MyLatinPhrasesViewModel()
        {
            LoadPhrases();
            FilterPhrasesCommand = new Xamarin.Forms.Command<string>(FilterPhrases);
            AddPhraseCommand = new Command(async () =>
            {
                var addPhrasePage = new AddPhrasePage(AddPhrase);
                DeletePhraseCommand = new Xamarin.Forms.Command<LatinPhrase>(DeletePhrase);

                var addPhraseNavigationPage = new NavigationPage(addPhrasePage);
                await Application.Current.MainPage.Navigation.PushModalAsync(addPhraseNavigationPage);
            });
            ShareCommand = new Xamarin.Forms.Command<LatinPhrase>(SharePhrase);
            EditPhraseCommand = new Xamarin.Forms.Command<LatinPhrase>(async (phrase) => await EditPhrase(phrase));
        }
        private void SavePhrases()
        {
            var json = JsonConvert.SerializeObject(Phrases);
            Preferences.Set("Phrases", json);
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
                    FilterPhrases(_searchText);
                }
            }
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
        public void CopyPhraseToClipboard(string phrase)
        {
            Xamarin.Essentials.Clipboard.SetTextAsync(phrase);
        }
        private ObservableCollection<LatinPhrase> LoadPhrasesFromStorage()
        {
            var json = Preferences.Get("Phrases", string.Empty);
            if (!string.IsNullOrEmpty(json))
            {
                return JsonConvert.DeserializeObject<ObservableCollection<LatinPhrase>>(json);
            }
            return null;
        }


        public void UpdatePhrase(LatinPhrase phrase)
        {
            var index = Phrases.IndexOf(phrase);
            if (index != -1)
            {
                Phrases[index] = phrase;
                SavePhrases();
            }
        }
        public async void DeletePhrase(LatinPhrase phrase)
        {
            bool confirmDelete = await Application.Current.MainPage.DisplayAlert("Kustuta fraas", "Kas olete kindel, et soovite selle fraasi kustutada?", "Jah", "Ei");
            
            if (confirmDelete)
            {
                
                Phrases.Remove(phrase);
                SavePhrases();
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
                    Phrases = new ObservableCollection<LatinPhrase>(
                        _allPhrases.Where(p => p.Latin.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    );
                }
                OnPropertyChanged(nameof(Phrases));
            
           
        }
        public async Task LoadPhrases()
        {
            _allPhrases = LoadPhrasesFromStorage();
            if (_allPhrases == null || _allPhrases.Count() == 0)
            {
                _allPhrases = new ObservableCollection<LatinPhrase>
        {
            new LatinPhrase { Latin = "Carpe diem", Estonian = "Haara päevast" },
            new LatinPhrase { Latin = "Veni, vidi, vici", Estonian = "Tulin, nägin, võitsin" },
            //... (Add other phrases here)
        };
            }

            Phrases = new ObservableCollection<LatinPhrase>(_allPhrases);
            OnPropertyChanged(nameof(Phrases));
        }
        public void AddPhrase(LatinPhrase newPhrase)
        {
           
                Phrases.Add(newPhrase);
                SavePhrases();
        }
        private async void SharePhrase(LatinPhrase phrase)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = $"{phrase.Latin} - {phrase.Estonian}",
                Title = "Share Latin Phrase"
            });
        }

        public async Task EditPhrase(LatinPhrase phrase)
        {
            var editPhrasePage = new EditPhrasePage(phrase, this);
            await Application.Current.MainPage.Navigation.PushModalAsync(editPhrasePage);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
