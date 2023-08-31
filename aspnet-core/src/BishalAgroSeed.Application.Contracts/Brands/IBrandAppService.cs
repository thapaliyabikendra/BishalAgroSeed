using BishalAgroSeed.Categories;
using BishalAgroSeed.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace BishalAgroSeed.Brands;
public interface IBrandAppService:ICrudAppService<BrandDto, Guid,PagedAndSortedResultRequestDto,CreateUpdateBrandDto>
{
   public Task<List<DropdownDto>> GetBrands();
}
