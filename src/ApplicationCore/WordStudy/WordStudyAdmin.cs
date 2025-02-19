namespace ApplicationCore.WordStudy;
public class WordStudyAdmin(IWordDepository wordDepository, IWordExplanationQuery wordExplanationQuery): IWordStudyAdmin
{
    public async Task<int> SetupWordDbAsync()
    {
        return await UploadWordListFromFileToDatabaseAsync();
    }



    private async Task<int> UploadWordListFromFileToDatabaseAsync()
    {
        var wordListFileName = "data/WordList-20250208c.txt";
        var wordList = await WordListHelper.ReadJsonFileToListAsync(wordListFileName);
        if (wordList is null || wordList.Count == 0) return 0;

        // findout word list where Name appears more than once
        var wordList2 = wordList.GroupBy(x => x.Name.ToLower()).Where(g => g.Count() > 1).Select(y => y.Key).ToList();

        var ret = await wordDepository.UpdateWordListAsync(wordList);
        return ret;
    }

    private async Task<int> RemoveDuplicateWordsAsync()
    {
        WordListProcess wordListProcess = new ();

        // load old version of word list from file        
        var wordListFileName = "WordList-20250208b.txt";
        var NewWordListFileName = "WordList-20250208c.txt";
        // update word list with old version db list
        var wordList = await WordListHelper.ReadJsonFileToListAsync(wordListFileName);
        if (wordList is null || wordList.Count == 0) return 0;
        var ret1 = wordListProcess.UpdateWordList(wordList);

        // save new word list to file
        await WordListHelper.WriteListToJsonFileAsync(NewWordListFileName, wordListProcess.GetWordList());

        return wordListProcess._wordDict.Count;

    }
    private async Task<int> SetupExplanationFromAiQuery()
    {
        WordListProcess wordListProcess = new ();

        // load old version of word list from file        
        var wordListFileName = "WordList-20250208b.txt";
        var NewWordListFileName = "WordList-20250208c.txt";
        // update word list with old version db list
        var wordList = await WordListHelper.ReadJsonFileToListAsync(wordListFileName);
        if (wordList is null || wordList.Count == 0) return 0;
        var ret1 = wordListProcess.UpdateWordList(wordList);
        
        var chunkSize=2;

        var batchList = wordListProcess._wordDict.Values.Where(x=>string.IsNullOrWhiteSpace(x.Pronunciation) || string.IsNullOrWhiteSpace(x.MeaningShort))
                            .Chunk(chunkSize);

        int count=0;
        foreach(var list in batchList)
        {
            try
            {
                var ret = await wordExplanationQuery.GetWordsExplanationAsync(list.ToList());
                wordListProcess.UpdateWordList(ret);
                count++;
                Console.WriteLine($"Processed {count} batch of {batchList.Count()} words");
                if(count%2==0)
                {
                    // save new word list to file
                    await WordListHelper.WriteListToJsonFileAsync(NewWordListFileName, wordListProcess.GetWordList());
                   
                }
                Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        // save new word list to file
        await WordListHelper.WriteListToJsonFileAsync(NewWordListFileName, wordListProcess.GetWordList());

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
