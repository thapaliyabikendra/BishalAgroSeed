using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace BishalAgroSeed.FiscalYears;
public interface IFiscalYearAppService : ICrudAppService<FiscalYearDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateFiscalYearDto>
{
}
