using ApplicationCore.WordStudy;
using ApplicationCore.WordStudy;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        return services;
    }

}
