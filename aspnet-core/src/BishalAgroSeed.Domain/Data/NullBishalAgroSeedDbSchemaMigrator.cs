using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace BishalAgroSeed.Data;

/* This is used if database provider does't define
 * IBishalAgroSeedDbSchemaMigrator implementation.
 */
public class NullBishalAgroSeedDbSchemaMigrator : IBishalAgroSeedDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
