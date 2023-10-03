using BishalAgroSeed.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace BishalAgroSeed.Products;
public interface IProductAppService : ICrudAppService<ProductDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateProductDto>
{
    public  Task<List<GetUnitTypeDto>> GetUnitTypesAsync();
    public Task<FileBlobDto> GetProductImageAsync(Guid id);

    /// <summary>
    /// Retrieves a list of products that match specific filtering criteria, and returns them in a paged and sorted format. 
    /// </summary>
    /// <param name="input">Pagination and sorting criteria</param>
    /// <param name="filter">Filtering criteria</param>
    /// <returns> Paged and sorted products </returns>
    public Task<PagedResultDto<ProductDto>> GetListByFilterAsync(PagedAndSortedResultRequestDto input, ProductFilter filter);
}