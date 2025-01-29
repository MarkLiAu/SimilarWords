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

    public async Task<int> SetupWordDbAsync()
    {
        return await SetupWordDbV2Async();
    }

    private async Task<int> SetupWordDbV2Async()
    {
        WordListProcess wordListProcess = new ();

        // load old version of word list from file        
        var wordListFileName = "WordList-20250123.txt";
        var NewWordListFileName = "WordList-20250123b.txt";

        // update word list with old version db list
        var wordList = await WordListHelper.ReadJsonFileToListAsync(wordListFileName);
        if (wordList is null || wordList.Count == 0) return 0;
        var ret1 = wordListProcess.UpdateWordList(wordList);


        //
        var newList = wordListProcess._wordDict.Select(x=>x.Value).OrderBy(x=>x.Frequency).ToList();

        // save new word list to file
        await WordListHelper.WriteListToJsonFileAsync(NewWordListFileName, newList);

        return newList.Count;
    }

    private async Task<int> SetupWordDbV1Async()
    {
        WordListProcess wordListProcess = new ();

        // load word frequency list
        var frequencyList = await WordListHelper.ReadWordFrequencyListAsync("Seq-COCAFrequency20000.txt"); 
        var ret1 = wordListProcess.UpdateWordList(frequencyList);
        wordListProcess.ReOrderWordFrequency();

        // load old version of word list from database to file        
        var wordListFileName = "WordList-20250122.txt";
        var NewWordListFileName = "WordList-20250123.txt";
        if(!File.Exists(wordListFileName)) 
        {
            var dbList = await wordDepository.GetWordListAsync();
            await WordListHelper.WriteListToJsonFileAsync(wordListFileName, dbList);
        }

        // update word list with old version db list
        var wordList = await WordListHelper.ReadJsonFileToListAsync(wordListFileName);
        if (wordList is null || wordList.Count == 0) return 0;
        var ret2 = wordListProcess.UpdateWordList(wordList);
        wordListProcess.ReOrderWordFrequency();

        var newList = wordListProcess._wordDict.Select(x=>x.Value).OrderBy(x=>x.Frequency).ToList();
        newList.UpdateAllSimilarWords();

        // save new word list to file
        await WordListHelper.WriteListToJsonFileAsync(NewWordListFileName, newList);

        return newList.Count;
    }

}
