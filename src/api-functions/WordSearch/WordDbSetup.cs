using ApplicationCore.WordDictionary;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SimilarWords.Infrastructure;

namespace api_functions.WordSearch
{
    public class WordDbSetup(ILogger<WordSearch> logger, IWordQuery wordQuery)
    {
        [Function(nameof(WordDbSetupAsync))]
        public async Task<IActionResult> WordDbSetupAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post",Route ="v1/admin/dbsetup")] 
            HttpRequest req)
        {
            var fileService = new WordDepositoryLocalFile();
            var wordList = await fileService.GetWordListAsync();
            wordList.UpdateAllSimilarWords();
            var result = await wordQuery.UpdateWordListAsync(wordList.ToList());
            return new OkObjectResult(result);
        }
    }
}
