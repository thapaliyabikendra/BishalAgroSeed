using AutoMapper;
using BishalAgroSeed.Brands;
using BishalAgroSeed.Categories;
using BishalAgroSeed.CompanyInfos;
using BishalAgroSeed.Configurations;
using BishalAgroSeed.Customers;
using BishalAgroSeed.Dtos;
using BishalAgroSeed.FiscalYears;
using BishalAgroSeed.NumberGenerations;
using BishalAgroSeed.OpeningBalances;
using BishalAgroSeed.Products;

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

        CreateMap<CreateUpdateCustomerDto, Customer>();
        CreateMap<Customer, CustomerDto>();

        CreateMap<CreateUpdateBrandDto, Brand>();
        CreateMap<Brand, BrandDto>();

        CreateMap<CreateUpdateCategoryDto, Category>();
        CreateMap<Category, CategoryDto>();

        CreateMap<CreateUpdateCompanyInfoDto, CompanyInfo>();
        CreateMap<CompanyInfo, CompanyInfoDto>();

        CreateMap<CreateUpdateProductDto, Product>()
            .ForMember(dest => dest.ImgFileName, opt =>
            {
                opt.MapFrom(src => src.File.FileName);
                opt.Condition(c => c.File != null);
            });
        CreateMap<Product, ProductDto>();

        CreateMap<CreateUpdateOpeningBalanceDto, OpeningBalance>();
        CreateMap<OpeningBalance, OpeningBalanceDto>();

        CreateMap<CreateUpdateNumberGenerationDto, NumberGeneration>();
        CreateMap<NumberGeneration, NumberGenerationDto>();

        CreateMap<CreateUpdateFiscalYearDto, FiscalYear>();
        CreateMap<FiscalYear, FiscalYearDto>();

        CreateMap<Category, Dtos.DropdownDto>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DisplayName));
    }
}
