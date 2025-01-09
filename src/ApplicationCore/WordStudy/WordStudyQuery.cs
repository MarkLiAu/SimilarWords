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

        await MergeUserStudyRecord(wordStudyList);
        return wordStudyList;
    }

    private async Task MergeUserStudyRecord(List<WordStudyModel> wordStudyList)
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
}
