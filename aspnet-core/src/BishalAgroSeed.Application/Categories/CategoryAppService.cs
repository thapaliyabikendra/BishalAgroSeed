using BishalAgroSeed.Dtos;
using BishalAgroSeed.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Validation;

namespace BishalAgroSeed.Categories;
[Authorize(BishalAgroSeedPermissions.Categories.Default)]
public class CategoryAppService : CrudAppService<Category, CategoryDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateCategoryDto>, ICategoryAppService
{
    public CategoryAppService(IRepository<Category, Guid> repository) : base(repository)
    {
        GetPolicyName = BishalAgroSeedPermissions.Categories.Default;
        GetListPolicyName = BishalAgroSeedPermissions.Categories.Default;
        CreatePolicyName = BishalAgroSeedPermissions.Categories.Create;
        UpdatePolicyName = BishalAgroSeedPermissions.Categories.Edit;
        DeletePolicyName = BishalAgroSeedPermissions.Categories.Delete;
    }

    public override async Task<CategoryDto> CreateAsync(CreateUpdateCategoryDto input)
    {
        if(await Repository.AnyAsync(s => s.DisplayName == input.DisplayName))
        {
            var msg = "Duplicate Category Name!!";
            throw new AbpValidationException(msg, new List<ValidationResult>()
            {
                new ValidationResult(msg, new [] {"displayName"})
            });
        }

        return await base.CreateAsync(input);
    }

    public async Task<List<DropdownDto>> GetCategoriesAsync(GetCategoryFilter filter)
    {
        //var categories = await Repository.GetListAsync();
        //var mappedCategories = ObjectMapper.Map<List<Category>, List<DropdownDto>>(categories);
        //return mappedCategories;
        var categoryQueryable = await Repository.GetQueryableAsync();
        var resp = categoryQueryable.Where(s => s.IsActive && s.Id != filter.Id).Select(s => new DropdownDto(s.Id.ToString().ToLower(), s.DisplayName)).ToList();
        return resp;
    }

    public override async Task<PagedResultDto<CategoryDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        if (string.IsNullOrWhiteSpace(input.Sorting))
        {
            input.Sorting = $"DisplayName";
        }
        var _categories = await Repository.GetQueryableAsync();
        var queryable = (from c in _categories
                         join p in _categories on c.ParentId equals p.Id into pj
                         from lj1 in pj.DefaultIfEmpty()
                         select new CategoryDto
                         {
                             Id = c.Id,
                             DisplayName = c.DisplayName,
                             ParentId = lj1 == null ? null : lj1.Id,
                             ParentName = lj1 == null ? null : lj1.DisplayName,
                             IsActive = c.IsActive
                         });

        var totalCount = queryable.Count();
        var data = queryable.Skip(input.SkipCount).Take(input.MaxResultCount).OrderBy(input.Sorting).ToList();
        return new PagedResultDto<CategoryDto>(totalCount, data);
    }

    public override async Task<CategoryDto> UpdateAsync(Guid id, CreateUpdateCategoryDto input)
    {
        var category = await Repository.FirstOrDefaultAsync(s => s.Id == id);
        if (category == null)
        {
            var msg = "Category not Found!!";
            throw new AbpValidationException(msg, new List<ValidationResult>() {
                new ValidationResult(msg, new [] { "id" })
            });
        }

        if (category.Id == input.ParentId)
        {
            var msg = "The parent category must differ from the category itself.";
            throw new AbpValidationException(msg, new List<ValidationResult>() {
                new ValidationResult(msg, new [] { "parentId" })
            });
        }

        if (await Repository.AnyAsync(s => s.DisplayName == input.DisplayName && s.Id == id))
        {
            var msg = "Duplicate category!!";
            throw new AbpValidationException(msg, new List<ValidationResult>() {
                new ValidationResult(msg, new []{"displayName"})
            });
        }

        return await base.UpdateAsync(id, input);
    }
}