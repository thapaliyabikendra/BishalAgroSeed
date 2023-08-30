using BishalAgroSeed.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace BishalAgroSeed.NumberGenerations;
[Authorize(BishalAgroSeedPermissions.NumberGenerations.Default)]
public class NumberGenerationAppService : CrudAppService<NumberGeneration, NumberGenerationDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateNumberGenerationDto>, INumberGenerationAppService
{
    public NumberGenerationAppService(IRepository<NumberGeneration, Guid> repository) : base(repository)
    {
        GetPolicyName = BishalAgroSeedPermissions.NumberGenerations.Default;
        GetListPolicyName = BishalAgroSeedPermissions.NumberGenerations.Default;
        CreatePolicyName = BishalAgroSeedPermissions.NumberGenerations.Create;
        CreatePolicyName = BishalAgroSeedPermissions.NumberGenerations.Edit;
        CreatePolicyName = BishalAgroSeedPermissions.NumberGenerations.Delete;
    }
}

