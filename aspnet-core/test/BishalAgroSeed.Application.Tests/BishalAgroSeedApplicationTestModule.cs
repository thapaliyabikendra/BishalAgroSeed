using Volo.Abp.Modularity;

namespace BishalAgroSeed;

[DependsOn(
    typeof(BishalAgroSeedApplicationModule),
    typeof(BishalAgroSeedDomainTestModule)
    )]
public class BishalAgroSeedApplicationTestModule : AbpModule
{

}
