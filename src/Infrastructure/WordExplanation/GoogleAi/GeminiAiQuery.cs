using System.Text.Json;
using ApplicationCore.WordStudy;
using Microsoft.Extensions.DependencyInjection;
namespace Infrastructure.WordExplanation;

public class GeminiAiQuery : IWordExplanationQuery
{
    private readonly GeminiService _geminiService;

    public GeminiAiQuery(IGeminiApi geminiApi)
    {
        _geminiService = new GeminiService(geminiApi);
    }

    public async Task<List<Word>> GetWordsExplanationAsync(List<Word> wordList)
    {
        // var prompt = "Explain English word '%0' with the following format: first line : The word's phonetic transcription using the International Phonetic Alphabet (IPA).; second line: A concise, descriptive explanation of the word's meaning and common usage in everyday language (not a formal dictionary definition), with a maximum character limit of 300 characters.; third line: an example sentence . result to be in json format";
        var prompt = "For each English word in the following json string of list of object where Name is the English Word, provide the following details in the same json schema: 1. Explanation (less than 300 characters), 2. Example sentence, 3. Pronunciation in the International Phonetic Alphabet (IPA).";
        var wordsJson = JsonSerializer.Serialize(wordList.Select(x => new { Name = x.Name, Pronounciation = "", MeaningShort = "", Example = "" }));
        const string model = "gemini-2.0-flash";
        var response = await _geminiService.GetResponseAsync([prompt, wordsJson], model);

        var idx = response.IndexOf("```json");
        if(idx<0) return wordList;
        var responseCleaned = response.Substring(idx+("```json").Length).Replace("\n", "").Replace("```", "");
        var responseList = JsonSerializer.Deserialize<List<Word>>(responseCleaned);

        if (responseList is null || responseList.Count == 0) return wordList;

        foreach(var word in wordList)
        {
            var responseWord = responseList.FirstOrDefault(x => x.Name == word.Name);
            if (responseWord is not null)
            {
                word.MeaningShort = responseWord.MeaningShort;
                word.Pronunciation = responseWord.Pronunciation;
                word.Example = responseWord.Example;
                word.LastUpdatedUtc = DateTime.UtcNow;
                Console.WriteLine($"Word: {word.Name}, Pronunciation: {word.Pronunciation}, Meaning: {word.MeaningShort}, Example: {word.Example}");
            }
        }

        return wordList;
    }
}