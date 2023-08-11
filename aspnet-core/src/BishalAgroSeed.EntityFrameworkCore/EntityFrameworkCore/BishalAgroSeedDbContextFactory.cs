using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BishalAgroSeed.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class BishalAgroSeedDbContextFactory : IDesignTimeDbContextFactory<BishalAgroSeedDbContext>
{
    public BishalAgroSeedDbContext CreateDbContext(string[] args)
    {
        BishalAgroSeedEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<BishalAgroSeedDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new BishalAgroSeedDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../BishalAgroSeed.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
