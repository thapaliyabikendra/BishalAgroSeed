using BishalAgroSeed.Dtos;
using BishalAgroSeed.Permissions;
using BishalAgroSeed.TranscationTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace BishalAgroSeed.TransactionTypes;
[Authorize(BishalAgroSeedPermissions.TransactionTypes.Default)]
public class TransactionTypeAppService : ApplicationService, ITransactionTypeAppService
{
    private readonly ILogger<TransactionTypeAppService> _logger;
    private readonly IRepository<TransactionType, Guid> _transactionTypeRepository;

    public TransactionTypeAppService(
         ILogger<TransactionTypeAppService> logger,
         IRepository<TransactionType, Guid> transactionTypeRepository
        )
    {
        _logger = logger;
        _transactionTypeRepository = transactionTypeRepository;
    }

    public async Task<List<DropdownDto>> GetTransactionTypes()
    {
        try
        {
            _logger.LogInformation($"TransactionTypeAppService.GetTransactionTypes - Started");

            var transactionTypes = await _transactionTypeRepository.GetQueryableAsync();
            var data = transactionTypes.Select(s => new DropdownDto(s.Id.ToString(), s.DisplayName)).ToList();

            _logger.LogInformation($"TransactionTypeAppService.GetTransactionTypes - Ended");
            return data;
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"TransactionTypeAppService.GetTransactionTypes - Exception : {ex}");
            throw;
        }
    }
}
