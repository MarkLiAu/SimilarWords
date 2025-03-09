using System.Security.Claims;
using ApplicationCore.WordStudy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api_webapi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class WordStudyController(ILogger<WordStudyController> logger, IWordStudyUpdate wordStudyUpdate, IWordStudyQuery wordStudyQuery) : ControllerBase
{
    [HttpGet("/api/wordstudy", Name = "GetUserCurrentWordStudyListAsync")]
    public async Task<IActionResult> GetUserCurrentWordStudyListAsync()
    {
        try
        {
            var userName = User.Identity?.Name
                    ?? User.FindFirst("preferred_username")?.Value
                    ?? User.FindFirst(ClaimTypes.Name)?.Value ;

            if(string.IsNullOrWhiteSpace(userName) && System.Diagnostics.Debugger.IsAttached) userName = "mark-local-test";

            var result = await wordStudyQuery.GetUserCurrentWordStudyListAsync(userName!);
            return new OkObjectResult(result);
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "GetUserCurrentWordStudyListAsync failed " );
            return new ObjectResult( new {error=ex.Message})
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }

    [HttpPost("/api/study/{name}", Name = "UpdateWordStudyAsync")]
    public async Task<IActionResult> UpdateWordStudyAsync(string name, int daysToStudy=0)
    {
        try
        {
            var userName = User.Identity?.Name
                    ?? User.FindFirst("preferred_username")?.Value
                    ?? User.FindFirst(ClaimTypes.Name)?.Value
                    ?? throw new Exception("User name not found in claims");

            if(string.IsNullOrWhiteSpace(userName) && System.Diagnostics.Debugger.IsAttached) userName = "mark-local-test";

            var result = await wordStudyUpdate.UpdateWordStudyAsync(userName, name, daysToStudy);
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

