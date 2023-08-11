using System.Threading.Tasks;

namespace BishalAgroSeed.Data;

public interface IBishalAgroSeedDbSchemaMigrator
{
    Task MigrateAsync();
}
