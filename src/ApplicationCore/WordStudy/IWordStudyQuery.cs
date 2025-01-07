using ApplicationCore.WordDictionary;

namespace ApplicationCore.WordStudyNameSpace;
public interface IWordStudyQuery
{
    Task<WordStudy> GetWordStudyAsync(string userName, string wordName);
}
