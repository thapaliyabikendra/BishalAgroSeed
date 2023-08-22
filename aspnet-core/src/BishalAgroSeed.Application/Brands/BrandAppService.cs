using BishalAgroSeed.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
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
}
