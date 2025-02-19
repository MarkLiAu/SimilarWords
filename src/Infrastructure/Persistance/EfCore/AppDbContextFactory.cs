using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var dbConnection = Environment.GetEnvironmentVariable("DbConnection");

        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseSqlServer(dbConnection);

        return new AppDbContext(builder.Options);
    }
}