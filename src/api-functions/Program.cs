using Infrastructure;
using Infrastructure.Persistance;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimilarWords.Infrastructure;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

builder.Services.AddHealthChecks()
  .AddCheck<ApiHealthCheck>("self")
  .AddDbContextCheck<AppDbContext>("sql_db");
builder.Services.AddInfrastructureSetup(builder.Configuration);

builder.Build().Run();
