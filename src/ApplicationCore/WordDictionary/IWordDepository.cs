namespace ApplicationCore.WordDictionary;

public interface IWordDepository
{
    Task<IList<Word>> GetWordListAsync();
    Task<IList<Word>> GetSimilarWordsAsync(string name);
    Task<int> UpdateWordAsync(Word word);
    Task<int> UpdateWordListAsync(Word[] wordList);
}