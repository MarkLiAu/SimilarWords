using ApplicationCore.WordStudy;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SimilarWords.Infrastructure;

namespace api_functions.Functions
{
    public class WordAdminFunctions(ILogger<WordAdminFunctions> logger, IWordStudyUpdate wordStudyUpdate)
    {
        [Function(nameof(WordDbSetupAsync))]
        public async Task<IActionResult> WordDbSetupAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post",Route ="v1/admin/dbsetup")] 
            HttpRequest req)
        {
            var fileService = new WordDepositoryLocalFile();
            var wordList = await fileService.GetWordListAsync();
            wordList.UpdateAllSimilarWords();
            var result = await wordStudyUpdate.UpdateWordListAsync(wordList.ToList());
            return new OkObjectResult(result);
        }
    }
}
