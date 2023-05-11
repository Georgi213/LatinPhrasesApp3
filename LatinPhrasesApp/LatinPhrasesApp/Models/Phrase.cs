using System;
using System.Collections.Generic;
using System.Text;

namespace LatinPhrasesApp.Models
{
    public class Phrase
    {
        public string Latin { get; set; }
        public string Estonian { get; set; }

        public bool IsFavorite { get; set; }
    }
}
