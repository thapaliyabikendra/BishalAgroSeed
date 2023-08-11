using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace BishalAgroSeed;

[Dependency(ReplaceServices = true)]
public class BishalAgroSeedBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "BishalAgroSeed";
}
