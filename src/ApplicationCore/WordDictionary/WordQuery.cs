
namespace ApplicationCore.WordDictionary;
public class WordQuery(IWordDepository wordDepository) : IWordQuery
{
    public async Task<IList<Word>> SearchSimilarWords(string searchText)
    {
        var wordList = await wordDepository.GetSimilarWordsAsync(searchText);
        return wordList;
    }
}
