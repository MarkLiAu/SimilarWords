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
        var response = await _geminiService.GetResponseAsync("what is 1+2");

        return wordList;
    }
}