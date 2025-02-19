using System.Threading.Tasks;
using Refit;

namespace Infrastructure.WordExplanation;
    public interface IGeminiApi
    {
        [Post("/v1beta/models/{model}:generateContent")]
        Task<GeminiResponse> GenerateContentAsync(string model, [Body] GeminiRequest request, [Query] string key);
    }
