using BishalAgroSeed.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace BishalAgroSeed.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(BishalAgroSeedEntityFrameworkCoreModule),
    typeof(BishalAgroSeedApplicationContractsModule)
    )]
public class BishalAgroSeedDbMigratorModule : AbpModule
{
}
