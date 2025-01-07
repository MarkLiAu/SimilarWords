using ApplicationCore.WordDictionary;

namespace ApplicationCore.WordStudyNameSpace;
public interface IWordStudyUpdate
{
    Task<int> UpdateWordStudyAsync(WordStudy wordStudy);
}
