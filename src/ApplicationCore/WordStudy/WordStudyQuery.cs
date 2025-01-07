using ApplicationCore.WordDictionary;

namespace ApplicationCore.WordStudyNameSpace;
public class WordStudyQuery(IWordDepository wordDepository) : IWordStudyQuery
{
    public async Task<WordStudy> GetWordStudyAsync(string userName, string wordName)
    {
        var result = await wordDepository.GetWordStudyAsync(userName, wordName);
        if(result is not null) return result;
        
        return new WordStudy(userName, wordName);
    }
}
