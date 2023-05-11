using LatinPhrasesApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using LatinPhrasesApp.Views;
using LatinPhrasesApp.ViewModels;
using LatinPhrasesApp.Services;

namespace LatinPhrasesApp.Data
{
    public class LatinPhraseData
    {
        public List<LatinPhrase> LatinPhrases { get; set; }


        public LatinPhraseData()
        {
            InitializeLatinPhrases();
        }

        private void InitializeLatinPhrases()
        {
            LatinPhrases = new List<LatinPhrase>
        {
            new LatinPhrase { Phrase = "Carpe diem", Translation = "Seize the day", Author = "Horace", Image = "phrase_papyrus1.png" },
            new LatinPhrase { Phrase = "Veni, vidi, vici", Translation = "I came, I saw, I conquered", Author = "Julius Caesar", Image = "phrase_papyrus2.png" },
            new LatinPhrase { Phrase = "Audentes fortuna iuvat", Translation = "Fortune favors the bold", Author = "Virgil", Image = "phrase_papyrus3.png" },
            // ... other Latin phrases
        };
            
        }
    }
}
