namespace ApplicationCore.WordStudy;
public class WordStudyAdmin(IWordDepository wordDepository, IWordExplanationQuery wordExplanationQuery): IWordStudyAdmin
{
    public async Task<int> SetupWordDbAsync()
    {
        return await SetupExplanationFromAiQuery();
    }

    private async Task<int> SetupExplanationFromAiQuery()
    {
        WordListProcess wordListProcess = new ();

        // load old version of word list from file        
        var wordListFileName = "WordList-20250123b.txt";
        var NewWordListFileName = "WordList-20250203.txt";
        // update word list with old version db list
        var wordList = await WordListHelper.ReadJsonFileToListAsync(wordListFileName);
        if (wordList is null || wordList.Count == 0) return 0;
        var ret1 = wordListProcess.UpdateWordList(wordList);
        
        var chunkSize=100;

        var batchList = wordListProcess._wordDict.Values.Where(x=>string.IsNullOrWhiteSpace(x.Pronounciation) || string.IsNullOrWhiteSpace(x.MeaningShort))
                            .Chunk(chunkSize);

        foreach(var list in batchList)
        {
            try
            {
                var ret = await wordExplanationQuery.GetWordsExplanationAsync(list.ToList());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        return wordListProcess._wordDict.Count;

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

    private async Task<int> Setup2kWordsFrequencyAsync()
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
