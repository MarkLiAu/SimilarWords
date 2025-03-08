using Infrastructure;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var dbConnection = Environment.GetEnvironmentVariable("DbConnection") ?? throw new Exception("DbConnection is not set");

        var builder = new DbContextOptionsBuilder<AppDbContext>();
        if (InfrastructureSetup.IsMsSqlConnection(dbConnection))
            builder.UseSqlServer(dbConnection);
        else if (InfrastructureSetup.IsMysqlConnection(dbConnection))
            builder.UseMySql(dbConnection, ServerVersion.AutoDetect(dbConnection));

        return new AppDbContext(builder.Options);
    }
}
