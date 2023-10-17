using BishalAgroSeed.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BishalAgroSeed.Trades;
public interface ITradeAppService
{
    /// <summary>
    /// Save Trade 
    /// </summary>
    /// <param name="input">Transaction data</param>
    public Task SaveTransactionAsync(CreateTransactionDto input);

    /// <summary>
    /// Get Trade Types
    /// </summary>
    /// <returns>List of Trade Types </returns>
    public Task<List<DropdownDto>> GetTradeTypes();
}
