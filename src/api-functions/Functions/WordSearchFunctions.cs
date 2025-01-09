using ApplicationCore.WordStudy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SimilarWords.Infrastructure;

namespace api_functions.Functions
{
    public class WordSearchFunctions(ILogger<WordSearchFunctions> logger, IWordStudyQuery wordQuery)
    {
        [Function(nameof(SearchSimilarWordsAsync))]
        public async Task<IActionResult> SearchSimilarWordsAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post",Route ="words/{name}")] 
            HttpRequest req, string name)
        {
            var claims = StaticWebAppsAuth.Parse(req);
            var result = await wordQuery.SearchSimilarWords(name, claims?.Identity?.Name);
            return new OkObjectResult(result);
        }
    }
}
