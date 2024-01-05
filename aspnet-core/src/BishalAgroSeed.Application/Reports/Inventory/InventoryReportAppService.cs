using BishalAgroSeed.Constants;
using BishalAgroSeed.Dtos;
using BishalAgroSeed.InventoryCounts;
using BishalAgroSeed.Products;
using BishalAgroSeed.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BishalAgroSeed.Reports.Inventory;
public class InventoryReportAppService : ApplicationService, IInventoryReportAppService
{
    private readonly IRepository<Product, Guid> _productRepository;
    private readonly IRepository<InventoryCount, Guid> _inventoryCountRepository;
    private readonly ILogger<InventoryReportAppService> _logger;
    private readonly IExcelService _excelService;
    private readonly Dictionary<string, Func<InventoryReportDto, object>> _mapconfig = new()
    {
        { "Product", item => item.ProductName },
        { "Count", item => item.Count },
        { "Count Date", item => item.CountDate }
    };

    public InventoryReportAppService(
    IRepository<Product, Guid> productRepository,
    IRepository<InventoryCount, Guid> inventoryCountRepository,
    ILogger<InventoryReportAppService> logger,
    IExcelService excelService)
    {
        _productRepository = productRepository;
        _inventoryCountRepository = inventoryCountRepository;
        _logger = logger;
        _excelService = excelService;
    }
    [HttpGet]
    public async Task<FileBlobDto> ExportExcelAsync(InventoryReportFilter filter)
    {
        try
        {
            _logger.LogInformation($"InventoryReportAppService.ExportExcelAsync - Started");

            var inventoryReports = await GetListDataByFilterAsync(filter);
            var content = await _excelService.ExportAsync(inventoryReports.ToList(), _mapconfig);
            var filename = string.Format(ExcelFileNames.INVENTORY_REPORT, $"_{DateTime.Now:yyyy/MM/dd HH:mm}");

            return new FileBlobDto(content, filename);
        }
        catch (Exception ex)
        {
            _logger.LogError($"InventoryReportAppService.ExportExcelAsync - ExceptionError - {ex}");
            throw;
        }
        finally
        {
            _logger.LogInformation($"InventoryReportAppService.ExportExcelAsync - Ended");
        }
    }
    public async Task<PagedResultDto<InventoryReportDto>> GetListByFilterAsync(PagedAndSortedResultRequestDto input, InventoryReportFilter filter)
    {
        try
        {
            _logger.LogInformation($"InventoryReportAppService.GetListByFilterAsync - Started");

            if (string.IsNullOrWhiteSpace(input.Sorting))
            {
                input.Sorting = "ProductName";
            }

            var data = await GetListDataByFilterAsync(filter);
            var dataCount = data.Count();
            var items = data.OrderBy(input.Sorting).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var result = new PagedResultDto<InventoryReportDto>(dataCount, items);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"InventoryReportAppService.GetListByFilterAsync - ExceptionError - {ex}");
            throw;
        }
        finally
        {
            _logger.LogInformation($"InventoryReportAppService.GetListByFilterAsync - Ended");
        }
    }
    private async Task<IQueryable<InventoryReportDto>> GetListDataByFilterAsync(InventoryReportFilter filter)
    {
        try
        {
            _logger.LogInformation($"InventoryReportAppService.GetListDataByFilterAsync - Started");

            // Trim
            filter.ProductName = filter.ProductName?.Trim()?.ToLower();

            var products = await _productRepository.GetQueryableAsync();
            var inventoryCounts = await _inventoryCountRepository.GetQueryableAsync();

            var data = (from p in products
                        join ic in inventoryCounts on p.Id equals ic.ProductId
                        where ic.CountDate.Date == filter.CountDate.Date
                        select new InventoryReportDto
                        {
                            ProductName = p.DisplayName,
                            Count = ic.Count,
                            CountDate = ic.CountDate
                        })
                        .WhereIf(!string.IsNullOrWhiteSpace(filter.ProductName), s => s.ProductName.ToLower().Contains(filter.ProductName));

            return data;
        }
        catch (Exception ex)
        {
            _logger.LogError($"InventoryReportAppService.GetListDataByFilterAsync - ExceptionError - {ex}");
            throw;
        }
        finally
        {
            _logger.LogInformation($"InventoryReportAppService.GetListDataByFilterAsync - Ended");
        }
    }
}

