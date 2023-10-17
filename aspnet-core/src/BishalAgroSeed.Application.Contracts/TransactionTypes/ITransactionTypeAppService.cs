using BishalAgroSeed.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BishalAgroSeed.TransactionTypes;
public interface ITransactionTypeAppService
{
    public Task<List<DropdownDto>> GetTransactionTypes();
}