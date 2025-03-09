using System.Security.Claims;
using ApplicationCore.WordStudy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api_webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class WordSearchController(ILogger<WordStudyController> logger, IWordStudyUpdate wordStudyUpdate, IWordStudyQuery wordStudyQuery) : ControllerBase
{

    [HttpGet("/api/words/{name}", Name = "SearchSimilarWordsAsync")]
    public async Task<IActionResult> SearchSimilarWordsAsync(string name)
    {
        try
        {
            var userName = User.Identity?.Name
                    ?? User.FindFirst("preferred_username")?.Value
                    ?? User.FindFirst(ClaimTypes.Name)?.Value ;

            if(string.IsNullOrWhiteSpace(userName) && System.Diagnostics.Debugger.IsAttached) userName = "mark-local-test";

            var result = await wordStudyQuery.SearchSimilarWords(name, userName);
            return new OkObjectResult(result);
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "SearchSimilarWordsAsync failed " );
            return new ObjectResult( new {error=ex.Message})
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }

}

