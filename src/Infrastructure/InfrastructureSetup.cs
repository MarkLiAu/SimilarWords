using ApplicationCore.WordStudy;
using Infrastructure.Persistance;
using Infrastructure.WordExplanation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Infrastructure;

public static class InfrastructureSetup
{
    public static IServiceCollection AddInfrastructureSetup(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistenceSetup(configuration);
        services.AddApplicationSetup(configuration);
        return services;
    }

    public static IServiceCollection AddPersistenceSetup(this IServiceCollection services, IConfiguration configuration)
    {
        // get connection string from environment variable
        var dbConnection = Environment.GetEnvironmentVariable("DbConnection");
        Console.WriteLine($"Connection String: {dbConnection}");
        if (!string.IsNullOrWhiteSpace(dbConnection) && dbConnection.ToLower().Contains("database.windows.net"))
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(dbConnection, sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
                });
            });
            services.AddScoped<IWordDepository, WordDepositoryEfCoreSql>();
        }
        else
        {
            services.AddScoped<IWordDepository, WordDepositoryLocalFile>();
        }
        return services;
    }
    public static IServiceCollection AddApplicationSetup(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IWordStudyQuery, WordStudyQuery>();
        services.AddScoped<IWordStudyUpdate, WordStudyUpdate>();
        services.AddScoped<IWordStudyAdmin, WordStudyAdmin>();

        services.AddRefitClient<IGeminiApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(Environment.GetEnvironmentVariable("GeminiAi:ApiBaseUrl") ?? throw new NullReferenceException("no GeminiAi:ApiBaseUrl setup")));
        services.AddScoped<IWordExplanationQuery, GeminiAiQuery>();
        return services;
    }

}
