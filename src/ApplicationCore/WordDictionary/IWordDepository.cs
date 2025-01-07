using ApplicationCore.WordStudyNameSpace;

namespace ApplicationCore.WordDictionary;

public interface IWordDepository
{
    Task<IList<Word>> GetWordListAsync();
    Task<IList<Word>> GetSimilarWordsAsync(string name);
    Task<int> UpdateWordAsync(Word word);
    Task<int> UpdateWordListAsync(IList<Word> wordList);

    Task<WordStudy?> GetWordStudyAsync(string userName, string wordName);
    Task<IList<WordStudy>> GetAllWordStudyAsync(string userName);
    Task<int> UpsertWordStudyAsync(WordStudy wordStudy);
}