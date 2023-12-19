using BishalAgroSeed.Constants;
using BishalAgroSeed.Customers;
using BishalAgroSeed.Dtos;
using BishalAgroSeed.LedgerAccounts;
using BishalAgroSeed.OpeningBalances;
using BishalAgroSeed.Products;
using BishalAgroSeed.Services;
using BishalAgroSeed.TransactionDetails;
using BishalAgroSeed.Transactions;
using BishalAgroSeed.TranscationTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace BishalAgroSeed.MovementAnalysis;
public class MovementAnalysisAppService : ApplicationService, IMovementAnalysisAppService
{
    private readonly IRepository<Transaction, Guid> _transactionRepository;
    private readonly IRepository<DateTimes.DateTime, Guid> _dateTimeRepository;
    private readonly IRepository<TransactionType, Guid> _transactionTypeRepository;
    private readonly IRepository<TransactionDetail, Guid> _transactionDetailRepository;
    private readonly IRepository<Customer, Guid> _customerRepository;
    private readonly IRepository<Product, Guid> _productRepository;
    private readonly ILogger<MovementAnalysisAppService> _logger;
    private readonly IExcelService _excelService;
    private static readonly Dictionary<string, Func<MovementAnalysisDto, object>> _mapConfig = new()
    {
       { "Particulars", item => item.Particulars},
       { "Purchases Quantity", item => item.Purchases?.Quantity},
       { "Purchases Eff. Rate ", item => item.Purchases?.EffRate},
       { "Purchases Value", item => item.Purchases?.Value},
       { "Sales Quantity", item => item.Sales?.Quantity},
       { "Sales Eff. Rate ", item => item.Sales?.EffRate},
       { "Sales Value ", item => item.Sales?.Value},
    };

    public MovementAnalysisAppService(
      IRepository<Transaction, Guid> transactionRepository,
       IRepository<DateTimes.DateTime, Guid> dateTimeRepository,
       ILogger<MovementAnalysisAppService> logger,
        IRepository<TransactionType, Guid> transactionTypeRepository,
        IRepository<TransactionDetail, Guid> transactionDetailRepository,
        IRepository<Customer, Guid> customerRepository,
        IRepository<Product, Guid> ProductRepository,
        IExcelService excelService)
    {
        _transactionRepository = transactionRepository;
        _dateTimeRepository = dateTimeRepository;
        _logger = logger;
        _transactionTypeRepository = transactionTypeRepository;
        _excelService = excelService;
        _transactionDetailRepository = transactionDetailRepository;
        _customerRepository = customerRepository;
        _productRepository = ProductRepository;
    }
    [HttpGet]
    public async Task<FileBlobDto> ExportExcelAsync(MovementAnalysisFilter filter)
    {
        try
        {
            _logger.LogInformation($"MovementAnalysisAppService.GetListByFilterAsync - Started");

            var data = await GetListDataByFilterAsync(filter);

            var content = await _excelService.ExportAsync(data.ToList(), _mapConfig);
            var fileName = string.Format(ExcelFileNames.MOVEMENT_ANALYSIS, $"_{DateTime.Now:yyyy/MM/dd HH:mm}");

            _logger.LogInformation($"MovementAnalysisAppService.GetListByFilterAsync - Ended");
            return new FileBlobDto(content, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"MovementAnalysisAppService.GetListByFilterAsync - Exception : {ex}");
            throw;
        }
    }

    public async Task<PagedResultDto<MovementAnalysisDto>> GetListByFilterAsync(PagedAndSortedResultRequestDto input, MovementAnalysisFilter filter)
    {
        try
        {
            _logger.LogInformation($"MovementAnalysisAppService.GetListByFilterAsync - Started");

            var data = await GetListDataByFilterAsync(filter);

            var dataCount = data.Count();
            var items = data.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var result = new PagedResultDto<MovementAnalysisDto>(dataCount, items);

            _logger.LogInformation($"MovementAnalysisAppService.GetListByFilterAsync - Ended");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"MovementAnalysisAppService.GetListByFilterAsync - Exception : {ex}");
            throw;
        }
    }

    private async Task<IQueryable<MovementAnalysisDto>> GetListDataByFilterAsync(MovementAnalysisFilter filter)
    {
        try
        {
            _logger.LogInformation($"MovementAnalysisAppService.GetListDataByFilterAsync - Started");
            var transactions = await _transactionRepository.GetQueryableAsync();
            var transactionTypes = await _transactionTypeRepository.GetQueryableAsync();
            var products = await _productRepository.GetQueryableAsync();
            var transactionDetails = await _transactionDetailRepository.GetQueryableAsync();

            var query = (from p in products
                         join td in transactionDetails on p.Id equals td.ProductId
                         join t in transactions on td.TransactionId equals t.Id
                         join tt in transactionTypes on t.TransactionTypeId equals tt.Id
                         where tt.Description == Constants.Global.TRANSACTION_TYPE_TRADE
                             && t.CustomerId == filter.CustomerId
                             && t.TranDate.Date >= filter.FromTranDate.Date
                             && t.TranDate.Date <= filter.ToTranDate.Date
                         group new { TradeType = tt.DisplayName, ProductName = p.DisplayName, td.Quantity, td.Price, Value = (td.Quantity * td.Price) }
                            by p.Id into g
                         select new MovementAnalysisDto
                         {
                             Particulars = g.FirstOrDefault().ProductName,
                             Purchases = g.Where(s => s.TradeType == Constants.TransactionTypes.PURCHASE).Count() < 0 ? null : new TradeMADto
                             {
                                 Quantity = g.Sum(x => x.Quantity),
                                 EffRate = g.Average(s => s.Price),
                                 Value = g.Sum(x => x.Value)
                             },
                             Sales = g.Where(s => s.TradeType == Constants.TransactionTypes.SALES).Count() < 0 ? null : new TradeMADto
                             {
                                 Quantity = g.Sum(x => x.Quantity),
                                 EffRate = g.Average(s => s.Price),
                                 Value = g.Sum(x => x.Value)
                             },
                         });

            return query;
        }

        catch (Exception ex)
        {
            _logger.LogInformation($"MovementAnalysisAppService.GetListDataByFilterAsync - Exception : {ex}");
            throw;
        }
    }
}
