using ApplicationCore.WordStudy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api_webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class WordAdminController(ILogger<WordAdminController> logger, IWordStudyAdmin wordStudyAdmin) : ControllerBase
{

    [HttpGet("/api/v1/admin/dbsetup", Name = "WordDbSetupAsync")]
    public async Task<IActionResult> WordDbSetupAsync()
    {
        try
        {
            var userName = User.Identity?.Name;
            if(string.IsNullOrWhiteSpace(userName) && System.Diagnostics.Debugger.IsAttached) userName = "mark-local-test";

            var result = await wordStudyAdmin.SetupWordDbAsync();
            return new OkObjectResult(result);
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "WordDbSetupAsync failed " );
            return new ObjectResult( new {error=ex.Message})
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }

}
