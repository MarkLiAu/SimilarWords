using ApplicationCore.WordDictionary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SimilarWords.Infrastructure;

namespace api_functions.WordSearch
{
    public class WordSearch(ILogger<WordSearch> logger, IWordQuery wordQuery)
    {
        [Function(nameof(SearchSimilarWordsAsync))]
        public async Task<IActionResult> SearchSimilarWordsAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post",Route ="words/{name}")] 
            HttpRequest req, string name)
        {
            var result = await wordQuery.SearchSimilarWords(name);
            var claims  = StaticWebAppsAuth.Parse(req);
            if (claims.Identity is null || !claims.Identity.IsAuthenticated)
            {
                result = result.Select(x=>x).ToList();
                foreach (var word in result)
                {
                    word.SoundUrl=string.Empty;
                }
            }   
            return new OkObjectResult(result);
        }
    }
}
