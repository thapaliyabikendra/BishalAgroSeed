using BishalAgroSeed.InventoryCounts;
using BishalAgroSeed.Products;
using Nito.AsyncEx;
using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using System.Linq;
using BishalAgroSeed.TransactionDetails;
using BishalAgroSeed.TranscationTypes;
using BishalAgroSeed.Transactions;
using Volo.Abp.Uow;
using BishalAgroSeed.MovementAnalysis;
using Microsoft.Extensions.Logging;

namespace BishalAgroSeed.Hangfire;

public class HangfireAppService : ISingletonDependency
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IRepository<InventoryCount, Guid> _inventoryCountRepository;
    private readonly IRepository<Product, Guid> _productRepository;
    private readonly IRepository<TransactionDetail, Guid> _transactionDetailRepository;
    private readonly IRepository<Transaction, Guid> _transactionRepository;
    private readonly IRepository<TransactionType, Guid> _transactionTypeRepository;
    private readonly ILogger<HangfireAppService> _logger;

    public HangfireAppService(
        IUnitOfWorkManager unitOfWorkManager,
         IRepository<InventoryCount, Guid> inventoryCountRepository,
         IRepository<Product, Guid> productRepository,
         IRepository<TransactionDetail, Guid> transactionDetailRepository,
         IRepository<Transaction, Guid> transactionRepository,
         IRepository<TransactionType, Guid> transactionTypeRepository,
         ILogger<HangfireAppService> logger
         )
    {
        _unitOfWorkManager = unitOfWorkManager;
        _inventoryCountRepository = inventoryCountRepository;
        _productRepository = productRepository;
        _transactionDetailRepository = transactionDetailRepository;
        _transactionRepository = transactionRepository;
        _transactionTypeRepository = transactionTypeRepository;
        _logger = logger;
    }
    public async Task SaveInventoryAsync()
    {
        try
        {
            _logger.LogInformation($"HangfireAppService.SaveInventoryAsync - Started");

            using (var uow = _unitOfWorkManager.Begin())
            {
                var products = await _productRepository.GetQueryableAsync();
                var transactions = await _transactionRepository.GetQueryableAsync();
                var transactionTypes = await _transactionTypeRepository.GetQueryableAsync();
                var transactionDetails = await _transactionDetailRepository.GetQueryableAsync();

                var inventoryCounts = (from p in products
                                       join td in transactionDetails on p.Id equals td.ProductId
                                       join t in transactions on td.TransactionId equals t.Id
                                       join tt in transactionTypes on t.TransactionTypeId equals tt.Id
                                       group new { TransactionType = tt.DisplayName, td.Quantity }
                                             by p.Id into g
                                       select new InventoryCount
                                       {
                                           ProductId = g.Key,
                                           Count = g.Where(s => s.TransactionType == Constants.TransactionTypes.PURCHASE).Sum(s => s.Quantity)
                                                 - g.Where(s => s.TransactionType == Constants.TransactionTypes.SALES).Sum(s => s.Quantity),
                                           CountDate = DateTime.Now,
                                       });
                await _inventoryCountRepository.InsertManyAsync(inventoryCounts);
                await uow.CompleteAsync();
            }
                _logger.LogInformation($"HangfireAppService.SaveInventoryAsync - Ended");
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"HangfireAppService.SaveInventoryAsync - Exception : {ex}");
            throw;
        }
    }
}
