using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LatinPhrasesApp.Models;
using MvvmHelpers;

namespace LatinPhrasesApp.ViewModels
{
    public class LetterPhrasesViewModel : BaseViewModel
    {
        private ObservableCollection<LatinPhrase> _phrases;
        private ObservableCollection<LatinPhrase> _allPhrases;

        public ObservableCollection<LatinPhrase> Phrases
        {
            get => _phrases;
            set
            {
                _phrases = value;
                OnPropertyChanged(nameof(Phrases));
            }
        }

        public LetterPhrasesViewModel(IEnumerable<LatinPhrase> phrases)
        {
            _allPhrases = new ObservableCollection<LatinPhrase>(phrases);
            Phrases = new ObservableCollection<LatinPhrase>(_allPhrases);
        }
        public LetterPhrasesViewModel()
        {
            Phrases = new ObservableCollection<LatinPhrase>();
        }
        public void FilterPhrasesByLetter(string letter)
        {
            if (_allPhrases != null)
            {
                Phrases = new ObservableCollection<LatinPhrase>(
                    _allPhrases.Where(p => p.Latin.StartsWith(letter, StringComparison.OrdinalIgnoreCase))
                );
                OnPropertyChanged(nameof(Phrases));
            }
        }
        public async Task LoadPhrases()
        {
            // Load phrases from storage or from your data source here
            // For example:
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
                 new LatinPhrase { Latin = "Laus propria sordet", Estonian = "Kiitus teie kasuks on rõve" },
                 new LatinPhrase { Latin = "Tempus edax rerum", Estonian = "Aja õgimine" },
                 new LatinPhrase { Latin = "In legibus salus", Estonian = "Pääste seaduses" },
                 new LatinPhrase { Latin = "Errando discimus", Estonian = "Vead õpetavad" },
        };

            Phrases = new ObservableCollection<LatinPhrase>(_allPhrases);
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

       
    }
}
