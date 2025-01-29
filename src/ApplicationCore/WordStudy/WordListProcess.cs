using System.Text.Json;

namespace ApplicationCore.WordStudy;
public class WordListProcess
{
    public Dictionary<string,Word> _wordDict = [];

    public int UpdateWordList(IList<Word> wordList)
    {
        int added=0,updated=0;
        var updateUtc = DateTime.UtcNow;
        foreach (var word in wordList)
        {
            if(word is null || word.Name is null || !IsWordValid(word.Name)) continue;
            var exist = _wordDict.TryGetValue(word.Name, out var oldWord);

            if(!exist || oldWord is null) oldWord = new Word(word.Name);

            // if(!exist) 
            //     Console.WriteLine($"new word: {word.Name} old: {oldWord.Frequency}, new: {word.Frequency}");
            if(exist && oldWord.Frequency!=word.Frequency && word.Frequency>0 && oldWord.Frequency>0)  
                Console.WriteLine($"different frequency old word: {oldWord.Name}, new word: {word.Name}, old: {oldWord.Frequency}, new: {word.Frequency}");

            var oldWordText = JsonSerializer.Serialize(oldWord);

            oldWord.MeaningShort = IsMeaningValid(word.MeaningShort) ? word.MeaningShort : oldWord.MeaningShort;
            oldWord.MeaningLong = IsMeaningValid(word.MeaningLong) ? word.MeaningLong : oldWord.MeaningLong;
            oldWord.Pronounciation = IsMeaningValid(word.Pronounciation) ? word.Pronounciation : oldWord.Pronounciation;
            oldWord.PronounciationAm = IsMeaningValid(word.PronounciationAm) ? word.PronounciationAm : oldWord.PronounciationAm;
            oldWord.SimilarWords = IsMeaningValid(word.SimilarWords) ? word.SimilarWords : oldWord.SimilarWords;
            // oldWord.SoundUrl = word.SoundUrl ?? oldWord.SoundUrl;
            // oldWord.ExampleSoundUrl = word.ExampleSoundUrl ?? oldWord.ExampleSoundUrl;
            // oldWord.Example = word.Example ?? oldWord.Example;

            oldWord.Frequency = oldWord.Frequency==0 ? word.Frequency : oldWord.Frequency;
            if(oldWord.Frequency<=0) oldWord.Frequency = 8000;

            var newWordText = JsonSerializer.Serialize(oldWord);
            if(oldWordText == newWordText) continue;

            oldWord.LastUpdatedUtc = updateUtc;
            _wordDict[word.Name] = oldWord;
            if(exist) updated++; else added++;  
        }

        return added + updated;
    }

    public void ReOrderWordFrequency()
    {
        int count=0;
        foreach(var word in _wordDict.Select(x=>x.Value).OrderBy(x=>x.Frequency)) 
        {
            word.Frequency=++count; 
        }
    }

    private static bool IsWordValid(string wordName)
    {
        if(string.IsNullOrWhiteSpace(wordName)) return false;
        if(wordName.Length>50) return false;

        return true;
    }

    private static bool IsMeaningValid(string? meaning)
    {
        if(string.IsNullOrWhiteSpace(meaning)) return false;
        if(double.TryParse(meaning, out _)) return false; 

        return true;
    }

}
