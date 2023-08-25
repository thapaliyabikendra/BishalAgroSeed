using BishalAgroSeed.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace BishalAgroSeed.CompanyInfos;
[Authorize(BishalAgroSeedPermissions.CompanyInfos.Default)]
public class CompanyInfoAppService : CrudAppService<CompanyInfo, CompanyInfoDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateCompanyInfoDto>, ICompanyInfoAppService
{
    public CompanyInfoAppService(IRepository<CompanyInfo, Guid> repository) : base(repository)
    {
        GetPolicyName = BishalAgroSeedPermissions.CompanyInfos.Default;
        GetListPolicyName = BishalAgroSeedPermissions.CompanyInfos.Create;
        CreatePolicyName = BishalAgroSeedPermissions.CompanyInfos.Edit;
        CreatePolicyName = BishalAgroSeedPermissions.CompanyInfos.Delete;
    }
}
