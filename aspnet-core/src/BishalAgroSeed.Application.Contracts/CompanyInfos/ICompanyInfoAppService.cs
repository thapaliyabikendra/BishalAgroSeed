using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace BishalAgroSeed.CompanyInfos;
public interface ICompanyInfoAppService : ICrudAppService<CompanyInfoDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateCompanyInfoDto>
{
}
