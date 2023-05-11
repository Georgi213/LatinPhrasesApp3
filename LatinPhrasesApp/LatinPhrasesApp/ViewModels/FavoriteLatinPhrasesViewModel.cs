using LatinPhrasesApp.Models;
using LatinPhrasesApp.Services;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;

namespace LatinPhrasesApp.ViewModels
{
    public class FavoriteLatinPhrasesViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<LatinPhrase> FavoritePhrases { get; set; }
        public ICommand RemoveFavoriteCommand { get; }
        public ICommand ShareCommand { get; }

        public FavoriteLatinPhrasesViewModel()
        {
            FavoritePhrases = new ObservableCollection<LatinPhrase>();
            RemoveFavoriteCommand = new Command<LatinPhrase>(RemoveFavorite);
            ShareCommand = new Command<LatinPhrase>(SharePhrase);
        }
        private async void SharePhrase(LatinPhrase phrase)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = $"{phrase.Latin} - {phrase.Estonian}",
                Title = "Share Latin Phrase"
            });
        }
        private void RemoveFavorite(LatinPhrase phrase)
        {
            if (FavoritePhrases.Contains(phrase))
            {
                FavoritePhrases.Remove(phrase);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
