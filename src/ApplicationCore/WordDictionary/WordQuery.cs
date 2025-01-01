using System.Net.Quic;

namespace ApplicationCore.WordDictionary;
public class WordQuery(IWordDepository wordDepository) : IWordQuery
{
    public async Task<IList<Word>> SearchSimilarWords(string searchText)
    {
        var wordList = await wordDepository.GetWordListAsync();
        var result = wordList.FindSimilarWords(searchText);
        return wordList;
    }
}
