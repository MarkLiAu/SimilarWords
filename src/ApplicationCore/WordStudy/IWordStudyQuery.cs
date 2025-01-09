using ApplicationCore.WordStudy;

namespace ApplicationCore.WordStudy;
public interface IWordStudyQuery
{
    Task<IList<WordStudyModel>> SearchSimilarWords(string searchText, string? userName = null);
    Task<WordStudyModel> GetWordStudyAsync(string userName, string wordName);
}
