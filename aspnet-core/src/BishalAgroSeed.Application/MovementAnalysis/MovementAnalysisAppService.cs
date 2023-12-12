using BishalAgroSeed.Customers;
using BishalAgroSeed.Dtos;
using BishalAgroSeed.LedgerAccounts;
using BishalAgroSeed.OpeningBalances;
using BishalAgroSeed.Services;
using BishalAgroSeed.TransactionDetails;
using BishalAgroSeed.Transactions;
using BishalAgroSeed.TranscationTypes;
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
    private readonly IExcelService _excelService;
    private readonly IRepository<TransactionDetail, Guid> _transactionDetailRepository;
    private readonly ILogger<MovementAnalysisAppService> _logger;

    public MovementAnalysisAppService(
      IRepository<Transaction, Guid> transactionRepository,
       IRepository<DateTimes.DateTime, Guid> dateTimeRepository,
       ILogger<MovementAnalysisAppService> logger,
        IRepository<TransactionType, Guid> transactionTypeRepository,
        IExcelService excelService,
        IRepository<TransactionDetail, Guid> transactionDetailRepository)
    {
        _transactionRepository = transactionRepository;
        _dateTimeRepository = dateTimeRepository;
        _logger = logger;
        _transactionTypeRepository = transactionTypeRepository;
        _excelService = excelService;
        _transactionDetailRepository = transactionDetailRepository;
    }

    public ILogger<MovementAnalysisAppService> Logger { get; }

    public Task<FileBlobDto> ExportExcelAsync(MovementAnalysisFilter filter)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResultDto<MovementAnalysisDto>> GetListByFilterAsync(PagedAndSortedResultRequestDto input, MovementAnalysisFilter filter)
    {
        throw new NotImplementedException();
    }
}
