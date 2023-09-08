using BishalAgroSeed.Customers;
using BishalAgroSeed.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Validation;

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

    public async override Task<FiscalYearDto> CreateAsync(CreateUpdateFiscalYearDto input)
    {
        // Trim
        input.DisplayName = input.DisplayName?.Trim();

        if (await Repository.AnyAsync(s => s.DisplayName == input.DisplayName))
        {
            var msg = "Duplicate Fiscal Year!!";
            throw new AbpValidationException(msg, new List<ValidationResult>()
            {
                new ValidationResult(msg, new [] {"displayName"})
            });
        }

        if (input.IsCurrent)
        {
            var fiscalYears = await Repository.GetListAsync();
            foreach (var fiscalYear in fiscalYears)
            {
                fiscalYear.IsCurrent = false;
            }
            await Repository.UpdateManyAsync(fiscalYears);
        }

        var resp = await base.CreateAsync(input);
        return resp;
    }

    public override async Task DeleteAsync(Guid id)
    {
        var fiscalYear = await Repository.FirstOrDefaultAsync(s => s.Id == id);
        if (fiscalYear == null)
        {
            var msg = "Fiscal Year not found!!";
            throw new AbpValidationException(msg, new List<ValidationResult>()
            {
              new ValidationResult(msg, new [] {"id"})
            });
        }

        if (fiscalYear.IsCurrent)
        {
            var msg = "Current Fiscal Year can't be delete!!";
            throw new AbpValidationException(msg, new List<ValidationResult>()
            {
              new ValidationResult(msg, new [] {"isCurrent"})
            });
        }

        await base.DeleteAsync(id);
    }

    public override async Task<FiscalYearDto> UpdateAsync(Guid id, CreateUpdateFiscalYearDto input)
    {
        // Trim
        input.DisplayName = input.DisplayName?.Trim();

        var fiscalYear = await Repository.FirstOrDefaultAsync(s => s.Id == id);
        if (fiscalYear == null)
        {
            var msg = "Fiscal Year not found!!";
            throw new AbpValidationException(msg, new List<ValidationResult>()
            {
              new ValidationResult(msg, new [] {"id"})
            });
        }

        var hasDisplayNameUpdated = fiscalYear.DisplayName != input.DisplayName;
        if (hasDisplayNameUpdated)
        {
            var hasDuplicateFiscalYear = await Repository.AnyAsync(s => s.Id != id && s.DisplayName == input.DisplayName);
            if (hasDuplicateFiscalYear)
            {
                var msg = "Duplicate Fiscal Year!!";
                throw new AbpValidationException(msg, new List<ValidationResult>()
                {
                    new ValidationResult(msg, new [] {"displayName"})
                });
            }
        }

        var hasIsCurrentUpdated = fiscalYear.IsCurrent != input.IsCurrent;
        if (hasIsCurrentUpdated && input.IsCurrent)
        {
            var fiscalYears = await Repository.GetListAsync();
            foreach (var fy in fiscalYears)
            {
                fy.IsCurrent = false;
            }
            await Repository.UpdateManyAsync(fiscalYears);
        }

        return await base.UpdateAsync(id, input);
    }
}