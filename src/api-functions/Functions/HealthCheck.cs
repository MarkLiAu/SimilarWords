using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace api_functions.Functions;
public class HealthCheckFunction
{
    private readonly HealthCheckService _healthCheckService;

    public HealthCheckFunction(HealthCheckService healthCheckService)
    {
        _healthCheckService = healthCheckService;
    }

    [Function("LiveCheck")]
    public async Task<HttpResponseData> LiveCheck(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health/live")] HttpRequestData req,
        FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger("HealthCheck");
        logger.LogInformation("Health check requested.");

        var healthReport = await _healthCheckService.CheckHealthAsync(check=>check.Tags.Contains("self"));

        var response = req.CreateResponse();
        response.Headers.Add("Content-Type", "application/json; charset=utf-8");

        if (healthReport.Status == HealthStatus.Healthy)
        {
            response.StatusCode = HttpStatusCode.OK;
            await response.WriteStringAsync("Healthy");
        }
        else
        {
            response.StatusCode = HttpStatusCode.ServiceUnavailable;
            await response.WriteStringAsync("Unhealthy");
        }

        return response;
    }

    [Function("HealthCheck")]
    public async Task<HttpResponseData> HealthCheck(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health")] HttpRequestData req,
        FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger("HealthCheck");
        logger.LogInformation("Health check requested.");

        var healthReport = await _healthCheckService.CheckHealthAsync();

        var response = req.CreateResponse();
        response.Headers.Add("Content-Type", "application/json; charset=utf-8");

        if (healthReport.Status == HealthStatus.Healthy)
        {
            response.StatusCode = HttpStatusCode.OK;
            await response.WriteStringAsync("Healthy");
        }
        else
        {
            response.StatusCode = HttpStatusCode.ServiceUnavailable;
            await response.WriteStringAsync("Unhealthy");
        }

        return response;
    }

}