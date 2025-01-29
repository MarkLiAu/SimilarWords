using ApplicationCore.WordStudy;

namespace ApplicationCore.WordStudy;
public interface IWordStudyUpdate
{
    Task<int> UpdateWordStudyAsync(WordStudyModel wordStudy);
    Task<int> UpdateWordStudyAsync(string userName, string wordName, int daysToStudy);

    Task<int> UpdateWordListAsync(IList<Word> wordList);
    Task<int> SetupWordDbAsync();
}
