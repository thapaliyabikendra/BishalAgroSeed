using BishalAgroSeed.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace BishalAgroSeed.Categories;
public interface ICategoryAppService: ICrudAppService<CategoryDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateCategoryDto>
{
    public Task<List<DropdownDto>> GetCategoriesAsync(GetCategoryFilter filter);
}