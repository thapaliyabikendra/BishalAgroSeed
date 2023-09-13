using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace BishalAgroSeed.CompanyInfos;
public interface ICompanyInfoAppService : ICrudAppService<CompanyInfoDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateCompanyInfoDto>
{
    public Task<PagedResultDto<CompanyInfoDto>> GetListByFilterAsync(PagedAndSortedResultRequestDto input, CompanyInfoFilter filter);
}
