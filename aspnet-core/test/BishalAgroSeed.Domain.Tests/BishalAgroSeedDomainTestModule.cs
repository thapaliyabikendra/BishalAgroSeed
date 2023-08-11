using BishalAgroSeed.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace BishalAgroSeed;

[DependsOn(
    typeof(BishalAgroSeedEntityFrameworkCoreTestModule)
    )]
public class BishalAgroSeedDomainTestModule : AbpModule
{

}
