using ApplicationCore.WordDictionary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace api_functions.WordSearch
{
    public class WordSearch(ILogger<WordSearch> logger, IWordQuery wordQuery)
    {
        [Function(nameof(SearchSimilarWordsAsync))]
        public async Task<IActionResult> SearchSimilarWordsAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post",Route ="words/{name}")] 
            HttpRequest req, string name)
        {
            var result = await wordQuery.SearchSimilarWords(name);
            return new OkObjectResult(result);
        }
    }
}
