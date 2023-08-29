using BishalAgroSeed.Dtos;
using BishalAgroSeed.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public async Task<List<DropdownDto>> GetCategoriesAsync(GetCategoryFilter filter) 
    {
        var getList = await Repository.GetListAsync();
        var mapped = ObjectMapper.Map<List<Category>, List<DropdownDto>>(getList);
        return mapped;
    }
}