using BishalAgroSeed.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace BishalAgroSeed.Trades;
public interface ITradeAppService
{
    /// <summary>
    /// Create Transaction Trade 
    /// </summary>
    /// <param name="input">Transaction data</param>
    public Task CreateAsync(CreateTransactionDto input);

    /// <summary>
    /// Get Trade Types
    /// </summary>
    /// <returns>List of Trade Types </returns>
    public Task<List<DropdownDto>> GetTradeTypes();
    public Task<PagedResultDto<TradeDto>> GetListByFilterAsync(PagedAndSortedResultRequestDto input, TradeFilter filter);
    public Task<FileBlobDto> ExportExcelAsync(TradeFilter filter);
    public Task<TransactionDto> GetAsync(Guid transactionId);
    public Task DeleteAsync(Guid transactionId);
}
