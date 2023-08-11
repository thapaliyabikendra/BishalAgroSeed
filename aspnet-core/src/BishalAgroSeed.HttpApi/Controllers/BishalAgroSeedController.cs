using BishalAgroSeed.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace BishalAgroSeed.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class BishalAgroSeedController : AbpControllerBase
{
    protected BishalAgroSeedController()
    {
        LocalizationResource = typeof(BishalAgroSeedResource);
    }
}
