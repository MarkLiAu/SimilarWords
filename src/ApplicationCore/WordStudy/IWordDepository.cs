
namespace ApplicationCore.WordStudy;

public interface IWordDepository
{
    Task<IList<Word>> GetWordListAsync();
    Task<IList<Word>> GetSimilarWordsAsync(string name);
    Task<int> UpdateWordAsync(Word word);
    Task<int> UpdateWordListAsync(IList<Word> wordList);

    Task<WordStudyModel?> GetWordStudyAsync(string userName, string wordName);
    Task<IList<Word>> GetMultipleWordSAsync(IEnumerable<string> wordList);
    Task<IList<WordStudyModel>> GetMultipleWordStudyAsync(string userName, IEnumerable<string> wordList);
    Task<IList<WordStudyModel>> GetUserWordStudyListAsync(string userName);
    Task<int> UpsertWordStudyAsync(WordStudyModel wordStudy);
}