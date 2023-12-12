using BishalAgroSeed.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace BishalAgroSeed.MovementAnalysis;
public interface IMovementAnalysisAppService
{
    public Task<PagedResultDto<MovementAnalysisDto>> GetListByFilterAsync(PagedAndSortedResultRequestDto input, MovementAnalysisFilter filter);
    public Task<FileBlobDto> ExportExcelAsync(MovementAnalysisFilter filter);
}
