using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace BishalAgroSeed.OpeningBalances;
public interface IOpeningBalanceAppService :ICrudAppService<OpeningBalanceDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateOpeningBalanceDto>
{
}
