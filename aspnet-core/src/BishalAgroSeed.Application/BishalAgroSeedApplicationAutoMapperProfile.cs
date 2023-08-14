using AutoMapper;
using BishalAgroSeed.Configurations;

namespace BishalAgroSeed;

public class BishalAgroSeedApplicationAutoMapperProfile : Profile
{
    public BishalAgroSeedApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<CreateUpdateConfigurationDto, Configuration>();
        CreateMap<Configuration, ConfigurationDto>();

    }
}
