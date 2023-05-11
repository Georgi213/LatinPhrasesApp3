using LatinPhrasesApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LatinPhrasesApp.Services
{
    public interface IDataService
    {
        Task<List<Author>> GetAuthorsAsync();
        Task<List<LatinPhrase>> GetLatinPhrasesByAuthorAsync(string authorName);
        Task<List<LatinPhrase>> GetFavoriteLatinPhrasesAsync();
        Task<IEnumerable<LatinPhrase>> GetLatinPhrasesAsync();
        Task<List<LatinPhrase>> GetMyLatinPhrasesAsync();
        Task<IEnumerable<LatinPhrase>> GetLatinPhrasesByAuthorIdAsync(int authorId);
        Task<bool> AddMyLatinPhraseAsync(LatinPhrase latinPhrase);
        Task<bool> UpdateMyLatinPhraseAsync(LatinPhrase latinPhrase);
        Task<bool> DeleteMyLatinPhraseAsync(int latinPhraseId);

        Task<IEnumerable<Author>> SearchAuthorsAsync(string searchText);

    }
}
