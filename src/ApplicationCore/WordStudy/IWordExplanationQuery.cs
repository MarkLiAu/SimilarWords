using ApplicationCore.WordStudy;

namespace ApplicationCore.WordStudy;
public interface IWordExplanationQuery
{
    Task<List<Word>> GetWordsExplanationAsync(List<Word> wordList);
}
