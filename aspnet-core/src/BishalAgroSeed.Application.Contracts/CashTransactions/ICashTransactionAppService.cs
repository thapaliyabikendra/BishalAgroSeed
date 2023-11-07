using BishalAgroSeed.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BishalAgroSeed.CashTransactions;
public interface ICashTransactionAppService
{
    /// <summary>
    /// Save Cash Transaction 
    /// </summary>
    /// <param name="input">Transaction data</param>
    public Task SaveTransactionAsync(CreateTransactionDto input);

    /// <summary>
    /// Get Cash Transaction Types
    /// </summary>
    /// <returns>List of Cash Transaction Types</returns>
    public Task <List<DropdownDto>> GetCashTransactionTypesAsync();
}
