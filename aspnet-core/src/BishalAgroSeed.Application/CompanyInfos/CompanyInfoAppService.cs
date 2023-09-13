using BishalAgroSeed.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Validation;

namespace BishalAgroSeed.CompanyInfos;
[Authorize(BishalAgroSeedPermissions.CompanyInfos.Default)]
public class CompanyInfoAppService : CrudAppService<CompanyInfo, CompanyInfoDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateCompanyInfoDto>, ICompanyInfoAppService
{
    public CompanyInfoAppService(IRepository<CompanyInfo, Guid> repository) : base(repository)
    {
        GetPolicyName = BishalAgroSeedPermissions.CompanyInfos.Default;
        GetListPolicyName = BishalAgroSeedPermissions.CompanyInfos.Default;
        CreatePolicyName = BishalAgroSeedPermissions.CompanyInfos.Create;
        UpdatePolicyName = BishalAgroSeedPermissions.CompanyInfos.Edit;
        DeletePolicyName = BishalAgroSeedPermissions.CompanyInfos.Delete;
    }

    public async Task<PagedResultDto<CompanyInfoDto>> GetListByFilterAsync(PagedAndSortedResultRequestDto input, CompanyInfoFilter filter)
    {
        if (string.IsNullOrWhiteSpace(input.Sorting))
        {
            input.Sorting = $"DisplayName";
        }

        // Trim
        filter.DisplayName = filter.DisplayName?.Trim();
        filter.Address = filter.Address?.Trim();
        filter.ContactNo = filter.ContactNo?.Trim();
        filter.PanNo = filter.PanNo?.Trim();

        var companyInfos = await Repository.GetQueryableAsync();
        var queryable = (
                          from ci in companyInfos
                          select new CompanyInfoDto
                          {
                              Id = ci.Id,
                              DisplayName = ci.DisplayName,
                              Address = ci.Address,
                              ContactNo = ci.ContactNo,
                              PanNo = ci.PanNo,
                          })
                          .WhereIf(!string.IsNullOrWhiteSpace(filter.DisplayName), s => s.DisplayName.ToLower().Contains(filter.DisplayName.ToLower()))
                          .WhereIf(!string.IsNullOrWhiteSpace(filter.Address), s => s.Address.ToLower().Contains(filter.Address.ToLower()))
                          .WhereIf(!string.IsNullOrWhiteSpace(filter.ContactNo), s => s.ContactNo.ToLower().Contains(filter.ContactNo.ToLower()))
                          .WhereIf(!string.IsNullOrWhiteSpace(filter.PanNo), s => s.PanNo.ToLower().Contains(filter.PanNo.ToLower()));

        var totalCount = queryable.Count();
        var data = queryable.Skip(input.SkipCount).Take(input.MaxResultCount).OrderBy(input.Sorting).ToList();
        return new PagedResultDto<CompanyInfoDto>(totalCount, data);
    }
    
    public override async Task<CompanyInfoDto> CreateAsync(CreateUpdateCompanyInfoDto input)
    {
        // Trim
        input.DisplayName = input.DisplayName?.Trim();
        input.Address = input.Address?.Trim();
        input.ContactNo = input.ContactNo?.Trim();
        input.PanNo = input.PanNo?.Trim();

        if (await Repository.AnyAsync(s => s.DisplayName == input.DisplayName))
        {
            var msg = "Duplicate Company Info !!";
            throw new AbpValidationException(msg, new List<ValidationResult>()
            {
                new  ValidationResult(msg, new [] {"displayName"})
            });
        }

        return await base.CreateAsync(input);
    }

    public override async Task<CompanyInfoDto> UpdateAsync(Guid id, CreateUpdateCompanyInfoDto input)
    {
        // Trim
        input.DisplayName = input.DisplayName?.Trim();
        input.Address = input.Address?.Trim();
        input.ContactNo = input.ContactNo?.Trim();
        input.PanNo = input.PanNo?.Trim();

        if (!(await Repository.AnyAsync(s => s.Id == id)))
        {
            var msg = "Company Info Not Found!!";
            throw new AbpValidationException(msg, new List<ValidationResult>() {
                new ValidationResult(msg, new [] {"id"})
            });
        }

        if (await Repository.AnyAsync(s => s.Id != id && s.DisplayName == input.DisplayName))
        {
            var msg = "Duplicate Company Info!!";
            throw new AbpValidationException(msg, new List<ValidationResult>() {
                new ValidationResult(msg, new [] {"displayName"})
            });
        }

        if (await Repository.AnyAsync(s => s.Id != id && s.PanNo == input.PanNo))
        {
            var msg = "Duplicate Pan Number!!";
            throw new AbpValidationException(msg, new List<ValidationResult>() {
                new ValidationResult(msg, new [] {"panNo"})
            });
        }

        return await base.UpdateAsync(id, input);
    }
}