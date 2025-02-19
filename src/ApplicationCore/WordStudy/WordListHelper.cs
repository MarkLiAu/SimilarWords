using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApplicationCore.WordStudy;
public static class WordListHelper
{
    public static async Task<IList<Word>?> ReadJsonFileToListAsync(string filePath) 
    {
        // deserialize _wordList from a file
        await using var stream = File.OpenRead(filePath);
        return await JsonSerializer.DeserializeAsync<IList<Word>>(stream);
    }

    public static async Task WriteListToJsonFileAsync(string filePath,IList<Word> wordList) 
    {
        JsonSerializerOptions jsonOptions = new() { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
        using (var stream = File.OpenWrite(filePath) )
        {
            await JsonSerializer.SerializeAsync(stream, wordList, jsonOptions );
        };
    }

    public static async Task<List<Word>> ReadWordFrequencyListAsync(string filePath)
    {
        //load the word frequency list from a file into a array  
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Word frequency list file not found.");
        }

        var wordFrequencyList = new List<Word>();
        var wordDict = new Dictionary<string, int>();
        int count=0;
        using (var streamReader = new StreamReader(filePath))
        {
            string? line;
            while ((line = await streamReader.ReadLineAsync()) != null)
            {
                var words = line.Split(" ");
                if(words.Length != 2||string.IsNullOrWhiteSpace(words[1])) continue;
                if(wordDict.ContainsKey(words[1].Trim())) 
                    continue;
                wordFrequencyList.Add(new Word( words[1].Trim()) { Frequency = ++count});
            }
        }
        return wordFrequencyList;
    }

}
