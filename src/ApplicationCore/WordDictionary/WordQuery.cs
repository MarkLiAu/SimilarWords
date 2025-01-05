using System.Net.Quic;

namespace ApplicationCore.WordDictionary;
public class WordQuery(IWordDepository wordDepository) : IWordQuery
{
    public async Task<IList<Word>> SearchSimilarWords(string searchText)
    {
        var wordList = await wordDepository.GetSimilarWordsAsync(searchText);
        return wordList;
    }

    public async Task<int> UpdateWordListAsync(IList<Word> wordList)
    {
        return await wordDepository.UpdateWordListAsync(wordList);
    }
}
