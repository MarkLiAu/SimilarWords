
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SimilarWords.Infrastructure;

public class ApiHealthCheck : IHealthCheck
{
  public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
  {
    return Task.FromResult(HealthCheckResult.Healthy("Healthy at " + System.DateTime.Now));
  }
}