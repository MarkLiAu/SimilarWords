using ApplicationCore.WordStudy;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SimilarWords.Infrastructure;

namespace api_functions.Functions
{
    public class WordAdminFunctions(ILogger<WordAdminFunctions> logger, IWordStudyAdmin wordStudyAdmin)
    {
        [Function(nameof(WordDbSetupAsync))]
        public async Task<IActionResult> WordDbSetupAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post",Route ="v1/admin/dbsetup")] 
            HttpRequest req)
        {
            var result = await wordStudyAdmin.SetupWordDbAsync();
            return new OkObjectResult(result);
        }
    }
}
