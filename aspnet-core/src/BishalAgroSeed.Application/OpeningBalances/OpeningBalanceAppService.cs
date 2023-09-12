using BishalAgroSeed.Categories;
using BishalAgroSeed.Customers;
using BishalAgroSeed.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace BishalAgroSeed.OpeningBalances;
[Authorize(BishalAgroSeedPermissions.OpeningBalances.Default)]
public class OpeningBalanceAppService : CrudAppService<OpeningBalance, OpeningBalanceDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateOpeningBalanceDto>, IOpeningBalanceAppService
{
    private readonly IRepository<Customer, Guid> _customerRepository;
    public OpeningBalanceAppService(IRepository<OpeningBalance, Guid> repository,
        IRepository<Customer, Guid> customerRepository) : base(repository)
    {
        GetPolicyName = BishalAgroSeedPermissions.OpeningBalances.Default;
        GetListPolicyName = BishalAgroSeedPermissions.OpeningBalances.Default;
        CreatePolicyName = BishalAgroSeedPermissions.OpeningBalances.Create;
        UpdatePolicyName = BishalAgroSeedPermissions.OpeningBalances.Edit;
        DeletePolicyName = BishalAgroSeedPermissions.OpeningBalances.Delete;
        _customerRepository = customerRepository;
    }

    public override async Task<PagedResultDto<OpeningBalanceDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        if (string.IsNullOrWhiteSpace(input.Sorting))
        {
            input.Sorting = $"TranDate desc";
        }

        var _openingBalances = await Repository.GetQueryableAsync();
        var _customers = await _customerRepository.GetQueryableAsync();
        var queryable = (
                          from ob in _openingBalances
                          join c in _customers on ob.CustomerId equals c.Id
                          select new OpeningBalanceDto
                          {
                              Id = ob.Id,
                              Amount = ob.Amount,
                              TranDate = ob.TranDate,
                              CustomerId = ob.CustomerId,
                              CustomerName = c.DisplayName,
                              IsReceivable = ob.IsReceivable,
                          });
        var totalCount = queryable.Count();
        var data = queryable.Skip(input.SkipCount).Take(input.MaxResultCount).OrderBy(input.Sorting).ToList();
        return new PagedResultDto<OpeningBalanceDto>(totalCount, data);
    }
}