using BishalAgroSeed.CycleCountDetails;
using BishalAgroSeed.NumberGenerations;
using BishalAgroSeed.Permissions;
using BishalAgroSeed.Products;
using BishalAgroSeed.TransactionDetails;
using BishalAgroSeed.Transactions;
using BishalAgroSeed.TranscationTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace BishalAgroSeed.CycleCounts;
[Authorize(BishalAgroSeedPermissions.CycleCounts.Default)]
public class CycleCountAppService : ApplicationService, ICycleCountAppService
{
    private readonly IRepository<NumberGeneration, Guid> _numberGenerationRepository;
    private readonly IRepository<CycleCount, Guid> _cycleCountRepository;
    private readonly IRepository<CycleCountDetail, Guid> _cycleCountDetailRepository;
    private readonly IRepository<Product, Guid> _productRepository;
    private readonly IRepository<Transaction, Guid> _transactionRepository;
    private readonly IRepository<TransactionType, Guid> _transactionTypeRepository;
    private readonly IRepository<TransactionDetail, Guid> _transactionDetailRepository;
    private readonly IRepository<IdentityUser, Guid> _identityUserRepository;
    private readonly ILogger<CycleCountAppService> _logger;

    public CycleCountAppService(
        IRepository<NumberGeneration, Guid> numberGenerationRepository,
        IRepository<CycleCount, Guid> cycleCountRepository,
        IRepository<CycleCountDetail, Guid> cycleCountDetailRepository,
        IRepository<Product, Guid> productRepository,
        IRepository<Transaction, Guid> transactionRepository,
        IRepository<TransactionType, Guid> transactionTypeRepository,
        IRepository<TransactionDetail, Guid> transactionDetailRepository,
        IRepository<IdentityUser, Guid> identityUserRepository,
        ILogger<CycleCountAppService> logger)
    {
        _numberGenerationRepository = numberGenerationRepository;
        _cycleCountRepository = cycleCountRepository;
        _cycleCountDetailRepository = cycleCountDetailRepository;
        _productRepository = productRepository;
        _transactionRepository = transactionRepository;
        _transactionTypeRepository = transactionTypeRepository;
        _transactionDetailRepository = transactionDetailRepository;
        _identityUserRepository = identityUserRepository;
        _logger = logger;
    }

    [Authorize(BishalAgroSeedPermissions.CycleCounts.Edit)]
    public Task BulkUpdateCycleCountDetailByExcelAsync(UpdateCycleCountDetailFileDto input)
    {
        throw new NotImplementedException();
    }

    [Authorize(BishalAgroSeedPermissions.CycleCounts.Edit)]
    public Task BulkUpdateCycleCountDetailUpdateAsync(List<UpdateCycleCountDetailDto> input)
    {
        throw new NotImplementedException();
    }

    [Authorize(BishalAgroSeedPermissions.CycleCounts.Close)]
    public async Task CloseAsync(Guid id)
    {
        _logger.LogInformation($"CycleCountAppService.CloseAsync - Started");
        var cycleCount = await _cycleCountRepository.FirstOrDefaultAsync(s => s.Id == id);
        if (cycleCount == null)
        {
            var msg = "Cycle Count Not Found !!";
            throw new AbpValidationException(msg, new List<ValidationResult>()
            {
                new  ValidationResult(msg, new [] {"id"})
            });
        }

        cycleCount.IsClosed = true;
        cycleCount.ClosedDate = DateTime.Now;
        cycleCount.ClosedBy = CurrentUser.Id;
        await _cycleCountRepository.UpdateAsync(cycleCount);
        _logger.LogInformation($"CycleCountAppService.CloseAsync - Updated CycleCount entity with Id : {cycleCount.Id}");

        _logger.LogInformation($"CycleCountAppService.CloseAsync - Ended");
    }

    [Authorize(BishalAgroSeedPermissions.CycleCounts.Create)]
    public async Task CreateAsync()
    {
        _logger.LogInformation($"CycleCountAppService.CreateAsync - Started");

        var numberGeneration = await _numberGenerationRepository.FirstOrDefaultAsync(s => s.NumberGenerationTypeId == NumberGenerationTypes.NumberGenerationTypes.CycleCount);
        if (numberGeneration == null)
        {
            var msg = "Cycle Count Number Generation is not setup!!";
            _logger.LogInformation($"CycleCountAppService.CreateAsync - Validation : {msg}");
            throw new AbpValidationException(msg, new List<ValidationResult>()
            {
                new  ValidationResult(msg, new [] {"numberGeneration"})
            });
        }

        ++numberGeneration.Number;
        var cciNumber = $"{numberGeneration.Prefix}{numberGeneration.Number}{numberGeneration.Suffix}";
        await _numberGenerationRepository.UpdateAsync(numberGeneration);
        _logger.LogInformation($"CycleCountAppService.CreateAsync - Updated NumberGeneration entity with Id : {numberGeneration.Id}");

        var cycleCount = new CycleCount
        {
            CCINumber = cciNumber
        };
        await _cycleCountRepository.InsertAsync(cycleCount);
        _logger.LogInformation($"CycleCountAppService.CreateAsync - Inserted CycleCount entity with Id : {cycleCount.Id}");

        var _transactions = await _transactionRepository.GetQueryableAsync();
        var _transactionTypes = await _transactionTypeRepository.GetQueryableAsync();
        var _transactionDetails = await _transactionDetailRepository.GetQueryableAsync();

        var cycleCountDetails = (
                                     from t in _transactions
                                     join tt in _transactionTypes on t.TransactionTypeId equals tt.Id
                                     join td in _transactionDetails on t.TransactionTypeId equals td.Id
                                     where tt.DisplayName == Constants.TransactionTypes.PURCHASE || tt.DisplayName == Constants.TransactionTypes.SALES
                                     group new { TransactionType = tt.DisplayName, td.Quantity } by new { td.ProductId } into g
                                     select new CycleCountDetail
                                     {
                                         ProductId = g.Key.ProductId,
                                         CycleCountId = cycleCount.Id,
                                         SystemQuantity = g.Where(s => s.TransactionType == Constants.TransactionTypes.PURCHASE).Sum(s => s.Quantity) -
                                                          g.Where(s => s.TransactionType == Constants.TransactionTypes.SALES).Sum(s => s.Quantity)
                                     }).ToList();

        if (!cycleCountDetails.Any())
        {
            var msg = "Empty Inventory!!";
            _logger.LogInformation($"CycleCountAppService.CreateAsync - Validation : {msg}");
            throw new AbpValidationException(msg, new List<ValidationResult>()
            {
                new  ValidationResult(msg, new [] {"productId"})
            });
        }

        await _cycleCountDetailRepository.InsertManyAsync(cycleCountDetails);
        _logger.LogInformation($"CycleCountAppService.CreateAsync - Bulk Inserted CycleCountDetail entity with CycleCountId : {cycleCount.Id}");

        _logger.LogInformation($"CycleCountAppService.CreateAsync - Ended");
    }

    [Authorize(BishalAgroSeedPermissions.CycleCounts.Default)]
    public async Task<CycleCountDto> GetAsync(Guid id)
    {
        _logger.LogInformation($"CycleCountAppService.GetAsync - Started");

        var _cycleCounts = await _cycleCountRepository.GetQueryableAsync();
        var _users = await _identityUserRepository.GetQueryableAsync();
        var cycleCount = (
                            from cc in _cycleCounts
                            join u in _users on cc.ClosedBy equals u.Id into ug
                            from uj in ug.DefaultIfEmpty()
                            where cc.Id == id
                            select new CycleCountDto
                            {
                                CCINumber = cc.CCINumber,
                                IsClosed = cc.IsClosed,
                                ClosedDate = cc.ClosedDate,
                                ClosedByName = uj == null ? null : uj.Name + " " + uj.Surname,
                                CreationTime = cc.CreationTime
                            }).FirstOrDefault();

        _logger.LogInformation($"CycleCountAppService.GetAsync - Ended");
        return cycleCount;
    }

    [Authorize(BishalAgroSeedPermissions.CycleCounts.Default)]
    public async Task<PagedResultDto<CycleCountDto>> GetListByFilterAsync(PagedAndSortedResultRequestDto input, CycleCountFilter filter)
    {
        _logger.LogInformation($"CycleCountAppService.GetListByFilterAsync - Started");

        // Trim
        filter.CCINumber = filter.CCINumber?.Trim()?.ToLower();
        filter.ClosedByName = filter.ClosedByName?.Trim()?.ToLower();

        if (string.IsNullOrWhiteSpace(input.Sorting))
        {
            input.Sorting = $"CreationTime desc";
        }

        var cycleCounts = await _cycleCountRepository.GetQueryableAsync();
        var users = await _identityUserRepository.GetQueryableAsync();
        var queryable = (from cc in cycleCounts
                         join u in users on cc.ClosedBy equals u.Id into ug
                         from uj in ug.DefaultIfEmpty()
                         select new CycleCountDto
                         {
                             CCINumber = cc.CCINumber,
                             IsClosed = cc.IsClosed,
                             ClosedDate = cc.ClosedDate,
                             ClosedByName = uj == null ? null : uj.Name + " " + uj.Surname,
                             CreationTime = cc.CreationTime
                         })
                         .WhereIf(!string.IsNullOrWhiteSpace(filter.CCINumber), s => s.CCINumber.ToLower().Contains(filter.CCINumber))
                         .WhereIf(filter.IsClosed.HasValue, s => s.IsClosed == filter.IsClosed)
                         .WhereIf(filter.ClosedFromDate.HasValue, s => s.ClosedDate.HasValue && s.ClosedDate.Value.Date >= filter.ClosedFromDate.Value.Date)
                         .WhereIf(filter.ClosedToDate.HasValue, s => s.ClosedDate.HasValue && s.ClosedDate.Value.Date <= filter.ClosedToDate.Value.Date)
                         .WhereIf(!string.IsNullOrWhiteSpace(filter.ClosedByName), s => s.ClosedByName.ToLower().Contains(filter.ClosedByName))
                         .WhereIf(filter.OpenedFromDate.HasValue, s => s.CreationTime.Date >= filter.OpenedFromDate.Value.Date)
                         .WhereIf(filter.OpenedToDate.HasValue, s => s.CreationTime.Date <= filter.OpenedToDate.Value.Date);

        var totalCount = queryable.Count();
        var data = queryable.Skip(input.SkipCount).Take(input.MaxResultCount).OrderBy(input.Sorting).ToList();
        _logger.LogInformation($"CycleCountAppService.GetListByFilterAsync - Ended");

        return new PagedResultDto<CycleCountDto>(totalCount, data);
    }

    [Authorize(BishalAgroSeedPermissions.CycleCounts.Default)]
    public async Task<PagedResultDto<CycleCountDetailDto>> GetCycleCountDetailListByFilterAsync(PagedAndSortedResultRequestDto input, CycleCountDetailFilter filter)
    {
        _logger.LogInformation($"CycleCountAppService.GetCycleCountDetailListByFilterAsync - Started");

        // Trim
        filter.ProductName = filter.ProductName?.Trim()?.ToLower();
        filter.Remarks = filter.Remarks?.Trim()?.ToLower();

        if (string.IsNullOrWhiteSpace(input.Sorting))
        {
            input.Sorting = $"CreationTime desc";
        }

        var cycleCountDetails = await _cycleCountDetailRepository.GetQueryableAsync();
        var products = await _productRepository.GetQueryableAsync();
        var queryable = (from cc in cycleCountDetails
                         join p in products on cc.ProductId equals p.Id
                         where cc.Id == filter.CycleCountId
                         select new CycleCountDetailDto
                         {
                             Id = cc.Id,
                             ProductId = cc.ProductId,
                             ProductName = p.DisplayName,
                             CycleCountId = cc.CycleCountId,
                             SystemQuantity = cc.SystemQuantity,
                             PhysicalQuantity = cc.PhysicalQuantity,
                             Remarks = cc.Remarks
                         })
                         .WhereIf(!string.IsNullOrWhiteSpace(filter.ProductName), s => s.ProductName.ToLower().Contains(filter.ProductName))
                         .WhereIf(!string.IsNullOrWhiteSpace(filter.Remarks), s => s.Remarks.ToLower().Contains(filter.Remarks));

        var totalCount = queryable.Count();
        var data = queryable.Skip(input.SkipCount).Take(input.MaxResultCount).OrderBy(input.Sorting).ToList();
        _logger.LogInformation($"CycleCountAppService.GetCycleCountDetailListByFilterAsync - Ended");

        return new PagedResultDto<CycleCountDetailDto>(totalCount, data);
    }
}