using LatinPhrasesApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatinPhrasesApp.Services
{
    public class DataService : IDataService
    {
        private readonly List<Author> _authors;
        private readonly List<LatinPhrase> _favoriteLatinPhrases;
        private readonly List<LatinPhrase> _myLatinPhrases;
        private readonly List<LatinPhrase> _latinPhrases;
        public DataService()
        {
            _authors = new List<Author>(); // Initialize the list of authors
            _favoriteLatinPhrases = new List<LatinPhrase>(); // Initialize the list of favorite Latin phrases
            _myLatinPhrases = new List<LatinPhrase>(); // Initialize the list of custom user-added Latin phrases
            _latinPhrases = new List<LatinPhrase>();
            // Add sample data to the authors and phrases lists
            // You can replace this with actual data retrieval logic
        }
       
        public Task<List<Author>> GetAuthorsAsync()
        {
            return Task.FromResult(_authors);
        }
        public async Task<IEnumerable<LatinPhrase>> GetLatinPhrasesByAuthorIdAsync(int authorId)
        {
            var filteredPhrases = _latinPhrases.Where(phrase => phrase.AuthorId == authorId);

            
            return await Task.FromResult(filteredPhrases);
        }
        public async Task<IEnumerable<LatinPhrase>> GetLatinPhrasesAsync()
        {
            return await Task.FromResult(_latinPhrases);
        }
        public Task<List<LatinPhrase>> GetLatinPhrasesByAuthorAsync(string authorName)
        {
            var latinPhrases = _authors.FirstOrDefault(a => a.Name == authorName)?.LatinPhrases;
            return Task.FromResult(latinPhrases?.Select((phrase, index) => new LatinPhrase
            {
                Author = authorName,
                Text = phrase,
                Translation = _authors.FirstOrDefault(a => a.Name == authorName)?.TranslatedPhrases[index]
            }).ToList() ?? new List<LatinPhrase>());
        }

        public Task<List<LatinPhrase>> GetFavoriteLatinPhrasesAsync()
        {
            return Task.FromResult(_favoriteLatinPhrases);
        }

        public Task<List<LatinPhrase>> GetMyLatinPhrasesAsync()
        {
            return Task.FromResult(_myLatinPhrases);
        }

        public Task<bool> AddMyLatinPhraseAsync(LatinPhrase latinPhrase)
        {
            _myLatinPhrases.Add(latinPhrase);
            return Task.FromResult(true);
        }

        public Task<bool> UpdateMyLatinPhraseAsync(LatinPhrase latinPhrase)
        {
            var existingPhrase = _myLatinPhrases.FirstOrDefault(p => p.Text == latinPhrase.Text && p.Author == latinPhrase.Author);
            if (existingPhrase != null)
            {
                existingPhrase.Translation = latinPhrase.Translation;
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
        public async Task<IEnumerable<Author>> SearchAuthorsAsync(string searchText)
        {

            var allAuthors = await GetAuthorsAsync(); // assuming you have a method to get all authors
            return allAuthors.Where(a => a.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        public Task<bool> DeleteMyLatinPhraseAsync(int latinPhraseId)
        {
            var latinPhrase = _myLatinPhrases.FirstOrDefault(p => p.Text.GetHashCode() == latinPhraseId);
            if (latinPhrase != null)
            {
                _myLatinPhrases.Remove(latinPhrase);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}
