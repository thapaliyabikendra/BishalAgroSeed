using BishalAgroSeed.Brands;
using BishalAgroSeed.Dtos;
using BishalAgroSeed.Permissions;
using BishalAgroSeed.UnitTypes;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace BishalAgroSeed.Products;
[Authorize(BishalAgroSeedPermissions.Products.Default)]
public class ProductAppService : CrudAppService<Product, ProductDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateProductDto>, IProductAppService
{
    private readonly IRepository<UnitType, Guid> _unitTypeRepository;

    public ProductAppService(IRepository<Product, Guid> repository,
        IRepository<UnitType, Guid> unitTypeRepository
        ) : base(repository)
    {
        GetPolicyName = BishalAgroSeedPermissions.Products.Default;
        GetListPolicyName = BishalAgroSeedPermissions.Products.Create;
        CreatePolicyName = BishalAgroSeedPermissions.Products.Edit;
        CreatePolicyName = BishalAgroSeedPermissions.Products.Delete;
        _unitTypeRepository = unitTypeRepository;
    }

    [Authorize(BishalAgroSeedPermissions.UnitTypes.Default)]
    public async Task<List<DropdownDto>> GetUnitTypes()
    {
        var unitTypeQueryable = await _unitTypeRepository.GetQueryableAsync();
        var resp = unitTypeQueryable.Select(s => new DropdownDto(s.Id.ToString().ToLower(), s.DisplayName)).ToList();
        return resp;
    }
}
