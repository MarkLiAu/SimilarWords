using System.Runtime.CompilerServices;

namespace ApplicationCore.WordStudy;
public class WordStudyQuery(IWordDepository wordDepository) : IWordStudyQuery
{
    public async Task<WordStudyModel> GetWordStudyAsync(string userName, string wordName)
    {
        var result = await wordDepository.GetWordStudyAsync(userName, wordName);
        if(result is not null) return result;
        
        return new WordStudyModel(userName, wordName);
    }

    public async Task<IList<WordStudyModel>> SearchSimilarWords(string searchText, string? userName = null)
    {
        if(string.IsNullOrWhiteSpace(userName) && System.Diagnostics.Debugger.IsAttached) userName = "mark-local-test";

        var wordList = await wordDepository.GetSimilarWordsAsync(searchText);

        var wordStudyList = wordList.Select(word => new WordStudyModel(userName, word)).ToList();

        await MergeUserStudyRecordAsync(wordStudyList);
        return wordStudyList;
    }

    public async Task<IList<WordStudyModel>> GetUserCurrentWordStudyListAsync(string userName)
    {
        int newWordsToStudy = 5;    // 
        var allStudyWords = await wordDepository.GetUserWordStudyListAsync(userName);

        var result = allStudyWords.Where(IsWordDue).ToList();

        var newWordCount = allStudyWords.Count(ws => ws.StudyCount == 1);
        if(newWordCount<newWordsToStudy)
            result.AddRange(allStudyWords.Where(ws => ws.StudyCount==0).Take(newWordsToStudy - newWordCount));
        await MergeWordRecordAsync(result);
        return result;
    }

    private static bool IsWordDue(WordStudyModel wordStudy, int hoursToStudyNewWord=2)
    {
        return wordStudy.StudyCount>1 && wordStudy.LastStudyTimeUtc.AddDays(wordStudy.DaysToStudy)>=DateTime.UtcNow
         || wordStudy.StudyCount==1 && wordStudy.LastStudyTimeUtc.AddHours(hoursToStudyNewWord)>=DateTime.UtcNow;
    }

    private async Task MergeUserStudyRecordAsync(List<WordStudyModel> wordStudyList)
    {
        if(wordStudyList.Count<=0 || string.IsNullOrWhiteSpace(wordStudyList[0].UserName) ) return;
        var userWordStudyList = await wordDepository.GetMultipleWordStudyAsync(wordStudyList[0].UserName!, wordStudyList.Select(x=>x.WordName!).ToList());
        foreach(var wordStudy in wordStudyList)
        {
            var userStudy = userWordStudyList.FirstOrDefault(x=>x.WordName ==wordStudy.WordName);
            if(userStudy is null) continue;
            wordStudy.LastStudyTimeUtc = userStudy.LastStudyTimeUtc;
            wordStudy.StartTimeUtc = userStudy.StartTimeUtc;
            wordStudy.IsClosed = userStudy.IsClosed;
            wordStudy.StudyCount = userStudy.StudyCount;
            wordStudy.DaysToStudy = userStudy.DaysToStudy;
            wordStudy.Id = userStudy.Id;
        }

    }

    private async Task MergeWordRecordAsync(List<WordStudyModel> wordStudyList)
    {
        if(wordStudyList.Count<=0) return;
        var wordList = await wordDepository.GetMultipleWordSAsync(wordStudyList.Select(x=>x.WordName!).ToList());
        foreach(var wordStudy in wordStudyList)
        {
            var w = wordList.FirstOrDefault(x=>x.Name == wordStudy.WordName);
            if(w is null) continue;
            wordStudy.Word=w;
        }

    }

}
