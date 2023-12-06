using BishalAgroSeed.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace BishalAgroSeed.LedgerAccounts;
public interface ILedgerAccountAppService
{
    public Task<PagedResultDto<LedgerAccountDto>> GetListByFilterAsync(PagedAndSortedResultRequestDto input, LedgerAccountFilter filter);
    public Task<FileBlobDto> ExportExcelAsync(LedgerAccountFilter filter);
}
