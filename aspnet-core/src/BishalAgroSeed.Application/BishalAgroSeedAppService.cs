using System;
using System.Collections.Generic;
using System.Text;
using BishalAgroSeed.Localization;
using Volo.Abp.Application.Services;

namespace BishalAgroSeed;

/* Inherit your application services from this class.
 */
public abstract class BishalAgroSeedAppService : ApplicationService
{
    protected BishalAgroSeedAppService()
    {
        LocalizationResource = typeof(BishalAgroSeedResource);
    }
}
