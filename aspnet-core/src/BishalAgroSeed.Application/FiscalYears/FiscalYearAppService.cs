using BishalAgroSeed.Customers;
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

namespace BishalAgroSeed.FiscalYears;
[Authorize(BishalAgroSeedPermissions.FiscalYears.Default)]
public class FiscalYearAppService : CrudAppService<FiscalYear, FiscalYearDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateFiscalYearDto>, IFiscalYearAppService
{
    public FiscalYearAppService(IRepository<FiscalYear, Guid> repository) : base(repository)
    {
        GetPolicyName = BishalAgroSeedPermissions.FiscalYears.Default;
        GetListPolicyName = BishalAgroSeedPermissions.FiscalYears.Default;
        CreatePolicyName = BishalAgroSeedPermissions.FiscalYears.Create;
        UpdatePolicyName = BishalAgroSeedPermissions.FiscalYears.Edit;
        DeletePolicyName = BishalAgroSeedPermissions.FiscalYears.Delete;
    }
}

