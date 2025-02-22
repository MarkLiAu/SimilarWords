using ApplicationCore.WordStudy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api_webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class WordStudyController(ILogger<WordStudyController> logger, IWordStudyUpdate wordStudyUpdate, IWordStudyQuery wordStudyQuery) : ControllerBase
{
    [HttpGet("/api/wordstudy", Name = "GetUserCurrentWordStudyListAsync")]
    public async Task<IActionResult> GetUserCurrentWordStudyListAsync()
    {
        try
        {
            var userName = User.Identity?.Name;
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

}

