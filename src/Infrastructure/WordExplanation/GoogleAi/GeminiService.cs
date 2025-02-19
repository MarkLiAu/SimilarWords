using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.WordExplanation;
public class GeminiService
{
  private readonly IGeminiApi _geminiApi;
  private readonly string _apiKey;

  public GeminiService(IGeminiApi geminiApi, string apiKey = "")
  {
    _geminiApi = geminiApi;
    // _apiKey = !string.IsNullOrWhiteSpace(apiKey) ? apiKey : Environment.GetEnvironmentVariable("GeminiAi:ApiKey") ?? throw new ArgumentNullException(nameof(apiKey));
  }

  public async Task<string> GetResponseAsync(List<string> prompts, string model = "gemini-2.0-flash-exp")
  {
    try
    {
      var request = new GeminiRequest
      {
        Contents = new[]
          {
                new GeminiContent
                {
                    Role = "user",
                    Parts = [.. prompts.Select(x=> new GeminiPart { Text = x })]
                }
            }
      };
      var requestJson = JsonSerializer.Serialize(request);
      var response = await _geminiApi.GenerateContentAsync(model, request, _apiKey);
      return response?.Candidates?[0]?.Content?.Parts?[0]?.Text ?? "No response received.";

    }
    catch (Exception ex)
    {
      Console.WriteLine($"GeminiService Error: {ex.Message}");
      return $"GeminiService Error: {ex.Message}";
    }
  }
}
