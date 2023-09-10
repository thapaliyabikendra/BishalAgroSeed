using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace BishalAgroSeed.OpeningBalances;
public interface IOpeningBalanceAppService :ICrudAppService<OpeningBalanceDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateOpeningBalanceDto>
{
}
