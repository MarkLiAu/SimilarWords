using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace SimilarWords.Function
{
    public class health
    {
        private readonly ILogger<health> _logger;

        public health(ILogger<health> logger)
        {
            _logger = logger;
        }

        [Function("health")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Healthy at " + System.DateTime.Now);
        }
    }
}
