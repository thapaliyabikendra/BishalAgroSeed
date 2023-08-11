using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace BishalAgroSeed;

[Dependency(ReplaceServices = true)]
public class BishalAgroSeedBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "BishalAgroSeed";
}
