using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.WordExplanation;
public class GeminiService
{
  private readonly IGeminiApi _geminiApi;
  private readonly string _apiKey;

  public GeminiService(IGeminiApi geminiApi, string apiKey = "")
  {
    _geminiApi = geminiApi;
    _apiKey = !string.IsNullOrWhiteSpace(apiKey) ? apiKey : Environment.GetEnvironmentVariable("GeminiAi:ApiKey") ?? throw new ArgumentNullException(nameof(apiKey));
  }

  public async Task<string> GetResponseAsync(string prompt, string model = "gemini-2.0-flash-exp")
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
                    Parts = new[]
                    {
                        new GeminiPart { Text = prompt }
                    }
                }
            }
      };

      var response = await _geminiApi.GenerateContentAsync(model, request, _apiKey);
      return response?.Candidates?[0]?.Content?.Parts?[0]?.Text ?? "No response received.";

    }
    catch (Exception ex)
    {
      return $"Error: {ex.Message}";
    }
  }
}


/*
  "prompt": "Explain English word 'leadership' with the following format: first line : The word's phonetic transcription using the International Phonetic Alphabet (IPA).; second line: A concise, descriptive explanation of the word's meaning and common usage in everyday language (not a formal dictionary definition), with a maximum character limit of 300 characters.; third line: an example sentence . result to be in json format",

*/