using ApplicationCore.WordDictionary;

namespace ApplicationCore.WordStudyNameSpace;
public class WordStudyUpdate(IWordDepository wordDepository): IWordStudyUpdate
{
    public async Task<int> UpdateWordStudyAsync(WordStudy wordStudy) 
    {
        wordStudy.LastStudyTimeUtc=DateTime.UtcNow;
        if(wordStudy.StartTimeUtc==DateTime.MinValue)
        {
            wordStudy.StartTimeUtc = wordStudy.LastStudyTimeUtc; // set to same time when it's 1st time
        }
        return await wordDepository.UpsertWordStudyAsync(wordStudy);
    }
}
