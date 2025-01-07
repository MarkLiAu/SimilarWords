using ApplicationCore.WordDictionary;
using ApplicationCore.WordStudyNameSpace;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SimilarWords.Infrastructure;

namespace api_functions.Functions;

public class WordStudyFunctions(ILogger<WordStudy> logger, IWordStudyUpdate wordStudyUpdate)
{
    [Function(nameof(UpdateWordStudyAsync))]
    public async Task<IActionResult> UpdateWordStudyAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post",Route ="study/{name}")] 
        HttpRequest req, string name, int daysToStudy=0)
    {
        try
        {
            var claims = StaticWebAppsAuth.Parse(req);
            if(claims.Identity is null || !claims.Identity.IsAuthenticated)
            {
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }
            WordStudy wordStudy = new(claims.Identity.Name!, name);
            wordStudy.DaysToStudy = daysToStudy;
            var result = await wordStudyUpdate.UpdateWordStudyAsync(wordStudy);
            return new OkObjectResult(result);
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "UpdateWordStudyAsync failed " );
            return new ObjectResult( new {error=ex.Message})
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}
