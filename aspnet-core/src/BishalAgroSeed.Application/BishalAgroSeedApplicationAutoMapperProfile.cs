using AutoMapper;
using BishalAgroSeed.Brands;
using BishalAgroSeed.Categories;
using BishalAgroSeed.CompanyInfos;
using BishalAgroSeed.Configurations;
using BishalAgroSeed.Customers;
using BishalAgroSeed.Dtos;
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

        CreateMap<CreateUpdateProductDto, Product>();
        CreateMap<Product, ProductDto>();
        CreateMap<Category, DropdownDto>()
            .ForMember(opt => opt.Value, opt => opt.MapFrom(dest => dest.Id.ToString()))
            .ForMember(opt => opt.Name, opt => opt.MapFrom(dest => dest.DisplayName));
    }
}
