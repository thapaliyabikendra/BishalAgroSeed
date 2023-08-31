using BishalAgroSeed.Dtos;
using BishalAgroSeed.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace BishalAgroSeed.Brands;
[Authorize(BishalAgroSeedPermissions.Brands.Default)]
public class BrandAppService : CrudAppService<Brand, BrandDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateBrandDto>, IBrandAppService
{
    public BrandAppService(IRepository<Brand, Guid> repository) : base(repository)
    {
        GetPolicyName = BishalAgroSeedPermissions.Brands.Default;
        GetListPolicyName = BishalAgroSeedPermissions.Brands.Default;
        CreatePolicyName = BishalAgroSeedPermissions.Brands.Create;
        CreatePolicyName = BishalAgroSeedPermissions.Brands.Edit;
        CreatePolicyName = BishalAgroSeedPermissions.Brands.Delete;
    }

    public async Task<List<DropdownDto>> GetBrands()
    {
        var brandQueryable = await Repository.GetQueryableAsync();
        var resp = brandQueryable.Select(s => new DropdownDto(s.Id.ToString().ToLower(), s.DisplayName)).ToList();
        return resp;
    }
}