using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace BishalAgroSeed.Brands;
public interface IBrandAppService:ICrudAppService<BrandDto, Guid,PagedAndSortedResultRequestDto,CreateUpdateBrandDto>
{
}
