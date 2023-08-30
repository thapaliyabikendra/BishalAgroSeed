using BishalAgroSeed.Categories;
using BishalAgroSeed.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace BishalAgroSeed.OpeningBalances;
[Authorize(BishalAgroSeedPermissions.OpeningBalances.Default)]
public class OpeningBalanceAppService : CrudAppService<OpeningBalance, OpeningBalanceDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateOpeningBalanceDto>, IOpeningBalanceAppService
{
    public OpeningBalanceAppService(IRepository<OpeningBalance, Guid> repository) : base(repository)
    {
        GetPolicyName = BishalAgroSeedPermissions.OpeningBalances.Default;
        GetListPolicyName = BishalAgroSeedPermissions.OpeningBalances.Default;
        CreatePolicyName = BishalAgroSeedPermissions.OpeningBalances.Create;
        CreatePolicyName = BishalAgroSeedPermissions.OpeningBalances.Edit;
        CreatePolicyName = BishalAgroSeedPermissions.OpeningBalances.Delete;
    }
}

