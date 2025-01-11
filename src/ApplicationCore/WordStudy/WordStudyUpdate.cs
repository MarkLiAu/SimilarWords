namespace ApplicationCore.WordStudy;
public class WordStudyUpdate(IWordDepository wordDepository): IWordStudyUpdate
{
    public async Task<int> UpdateWordStudyAsync(WordStudyModel wordStudy) 
    {
        wordStudy.LastStudyTimeUtc=DateTime.UtcNow;
        if(wordStudy.StartTimeUtc==DateTime.MinValue)
        {
            wordStudy.StartTimeUtc = wordStudy.LastStudyTimeUtc; // set to same time when it's 1st time
        }
        return await wordDepository.UpsertWordStudyAsync(wordStudy);
    }

    public async Task<int> UpdateWordStudyAsync(string userName, string wordName, int daysToStudy) 
    {
        var wordStudy = await wordDepository.GetWordStudyAsync(userName, wordName);
        if (wordStudy == null)
        {
            // bookmark the word
            wordStudy = new WordStudyModel(userName, wordName);
            wordStudy.LastStudyTimeUtc = DateTime.UtcNow; 
            wordStudy.StartTimeUtc = wordStudy.LastStudyTimeUtc;
            wordStudy.IsClosed = false;
            wordStudy.StudyCount=0;
            wordStudy.DaysToStudyHistory=string.Empty;
        }
        else
        {
            wordStudy.DaysToStudyHistory += $"{wordStudy.DaysToStudy}/{(DateTime.UtcNow - wordStudy.LastStudyTimeUtc).TotalDays.ToString("0.00")}/{daysToStudy}, ";
            wordStudy.LastStudyTimeUtc = DateTime.UtcNow; 
            wordStudy.StudyCount++;
        }
        wordStudy.DaysToStudy = daysToStudy;
        return await wordDepository.UpsertWordStudyAsync(wordStudy);
    }

    public async Task<int> UpdateWordListAsync(IList<Word> wordList)
    {
        return await wordDepository.UpdateWordListAsync(wordList);
    }
}
