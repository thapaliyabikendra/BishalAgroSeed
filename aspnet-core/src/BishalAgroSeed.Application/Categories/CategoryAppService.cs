using BishalAgroSeed.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace BishalAgroSeed.Categories;
[Authorize(BishalAgroSeedPermissions.Categories.Default)]
public class CategoryAppService : CrudAppService<Category, CategoryDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateCategoryDto>, ICategoryAppService
{
    public CategoryAppService(IRepository<Category, Guid> repository) : base(repository)
    {
        GetPolicyName = BishalAgroSeedPermissions.Categories.Default;
        GetListPolicyName = BishalAgroSeedPermissions.Categories.Default;
        CreatePolicyName = BishalAgroSeedPermissions.Categories.Create;
        CreatePolicyName = BishalAgroSeedPermissions.Categories.Edit;
        CreatePolicyName = BishalAgroSeedPermissions.Categories.Delete;
    }
}