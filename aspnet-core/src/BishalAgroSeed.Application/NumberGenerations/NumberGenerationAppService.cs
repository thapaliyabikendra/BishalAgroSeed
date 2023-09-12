using BishalAgroSeed.Categories;
using BishalAgroSeed.Dtos;
using BishalAgroSeed.NumberGenerationTypes;
using BishalAgroSeed.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Validation;

namespace BishalAgroSeed.NumberGenerations;
[Authorize]
public class NumberGenerationAppService : CrudAppService<NumberGeneration, NumberGenerationDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateNumberGenerationDto>, INumberGenerationAppService
{
    public NumberGenerationAppService(IRepository<NumberGeneration, Guid> repository) : base(repository)
    {
        GetPolicyName = BishalAgroSeedPermissions.NumberGenerations.Default;
        GetListPolicyName = BishalAgroSeedPermissions.NumberGenerations.Default;
        CreatePolicyName = BishalAgroSeedPermissions.NumberGenerations.Create;
        UpdatePolicyName = BishalAgroSeedPermissions.NumberGenerations.Edit;
        DeletePolicyName = BishalAgroSeedPermissions.NumberGenerations.Delete;
    }

    public override async Task<NumberGenerationDto> CreateAsync(CreateUpdateNumberGenerationDto input) 
    {
        if (await Repository.AnyAsync(s => s.NumberGenerationTypeId == input.NumberGenerationTypeId))
        {
            var msg = "Duplicate Number Generation Type!!";
            throw new AbpValidationException(msg, new List<ValidationResult>()
            {
                new  ValidationResult(msg, new [] {"numberGenerationTypeId"})
            });
        }

        return await base.CreateAsync(input);
    }

    [Authorize(BishalAgroSeedPermissions.NumberGenerations.Default)]
    public override async Task<PagedResultDto<NumberGenerationDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        if (string.IsNullOrWhiteSpace(input.Sorting))
        {
            input.Sorting = $"NumberGenerationTypeId";
        }
        
        var numberGenerations  = await Repository.GetQueryableAsync();
        var queryable = (
                          from ng in numberGenerations
                          select new NumberGenerationDto
                          {
                              Id = ng.Id,
                              Prefix = ng.Prefix,
                              Number = ng.Number,
                              Suffix = ng.Suffix,
                              NumberGenerationTypeId = ng.NumberGenerationTypeId,
                              NumberGenerationTypeName = ng.NumberGenerationTypeId.ToString(),
                          });
        var totalCount = queryable.Count();
        var data = queryable.Skip(input.SkipCount).Take(input.MaxResultCount).OrderBy(input.Sorting).ToList();
        return new PagedResultDto<NumberGenerationDto>(totalCount, data);
    }

    [Authorize(BishalAgroSeedPermissions.NumberGenerationTypes.Default)]
    public async Task<List<DropdownDto>> GetNumberGenerationTypesAsync()
    {
        return await Task.Factory.StartNew(() => 
                    Enum.GetValues<NumberGenerationTypes.NumberGenerationTypes>()
                    .Select(s => new DropdownDto(((int)s).ToString(), s.ToString())).ToList());
    }

    public override async Task<NumberGenerationDto> UpdateAsync(Guid id, CreateUpdateNumberGenerationDto input)
    {
        if (!(await Repository.AnyAsync(s => s.Id == id)))
        {
            var msg = "Number Generation Not Found!!";
            throw new AbpValidationException(msg, new List<ValidationResult>() {
                new ValidationResult(msg, new [] {"id"})
            });
        }

        if (await Repository.AnyAsync(s => s.Id != id && s.NumberGenerationTypeId == input.NumberGenerationTypeId))
        {
            var msg = "Duplicate Number Generation Type!!";
            throw new AbpValidationException(msg, new List<ValidationResult>() {
                new ValidationResult(msg, new [] {"numberGenerationTypeId"})
            });
        }

        return await base.UpdateAsync(id, input);
    }
}

