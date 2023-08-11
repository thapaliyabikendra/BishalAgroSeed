using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BishalAgroSeed.Data;
using Volo.Abp.DependencyInjection;

namespace BishalAgroSeed.EntityFrameworkCore;

public class EntityFrameworkCoreBishalAgroSeedDbSchemaMigrator
    : IBishalAgroSeedDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreBishalAgroSeedDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the BishalAgroSeedDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<BishalAgroSeedDbContext>()
            .Database
            .MigrateAsync();
    }
}
