using BishalAgroSeed.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace BishalAgroSeed.CycleCounts;
public interface ICycleCountAppService
{
    /// <summary>
    /// Retrieves a cycle count item based on a specified unique identifier (ID).
    /// </summary>
    /// <param name="id">Unique identifier</param>
    /// <returns>Cycle count item</returns>
    public Task<CycleCountDto> GetAsync(Guid id);

    /// <summary>
    /// Retrieves a list of cycle count items that match specific filtering criteria, and returns them in a paged and sorted format. 
    /// </summary>
    /// <param name="input">Pagination and sorting criteria</param>
    /// <param name="filter">Filtering criteria</param>
    /// <returns> Paged and sorted cycle count items</returns>
    public Task<PagedResultDto<CycleCountDto>> GetListByFilterAsync(PagedAndSortedResultRequestDto input, CycleCountFilter filter);

    /// <summary>
    /// Creates a new cycle count item.
    /// </summary>
    public Task CreateAsync();

    /// <summary>
    /// Closes an existing cycle count item specified by its unique identifier (ID).
    /// </summary>
    /// <param name="id">Unique identifier</param>
    public Task CloseAsync(Guid id);

    /// <summary>
    /// Retrieves a list of cycle count details based on specified filtering criteria, returning the results in a paged and sorted format.
    /// </summary>
    /// <param name="input">Pagination and sorting criteria</param>
    /// <param name="filter">Filtering criteria</param>
    /// <returns>Paged and sorted cycle count items</returns>
    public Task<PagedResultDto<CycleCountDetailDto>> GetCycleCountDetailListByFilterAsync(PagedAndSortedResultRequestDto input, CycleCountDetailFilter filter);

    /// <summary>
    ///  Performs bulk updates on cycle count details using a list of update objects.
    /// </summary>
    /// <param name="cycleCountId">Cycle Count Id</param>
    /// <param name="input">List of update dto</param>
    public Task BulkUpdateCycleCountDetailAsync(Guid cycleCountId, List<UpdateCycleCountDetailDto> input);

    /// <summary>
    /// Downloads a Bulk Cycle Count Detail excel file template
    /// </summary>
    /// <returns>Downloads a excel file</returns>
    public Task<FileBlobDto> DownloadBulkUpdateCycleCountDetailByExcelTemplateAsync();

    /// <summary>
    /// Performs bulk updates on cycle count details using data from an Excel file.
    /// </summary>
    /// <param name="input">Excel file dto</param>
    /// <returns></returns>
    public Task BulkUpdateCycleCountDetailByExcelAsync(UpdateCycleCountDetailFileDto input);

    /// <summary>
    /// Exports a Cycle Count Detail excel file
    /// </summary>
    /// <param name="filter">Filtering criteria</param>
    /// <returns>Downloads a excel file</returns>
    public Task<FileBlobDto> ExportCycleCountDetailExcelAsync(CycleCountDetailFilter filter);
}