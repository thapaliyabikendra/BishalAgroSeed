using BishalAgroSeed.Configurations;
using BishalAgroSeed.Dtos;
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

namespace BishalAgroSeed.Customers;
[Authorize(BishalAgroSeedPermissions.Customers.Default)]
public class CustomerAppService : CrudAppService<Customer, CustomerDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateCustomerDto>, ICustomerAppService
{
    public CustomerAppService(IRepository<Customer, Guid> repository) : base(repository)
    {
        GetPolicyName = BishalAgroSeedPermissions.Customers.Default;
        GetListPolicyName = BishalAgroSeedPermissions.Customers.Default;
        CreatePolicyName = BishalAgroSeedPermissions.Customers.Create;
        UpdatePolicyName = BishalAgroSeedPermissions.Customers.Edit;
        DeletePolicyName = BishalAgroSeedPermissions.Customers.Delete;
    }

    public async Task<List<DropdownDto>> GetCustomersAsync()
    {
        var customerQueryable = await Repository.GetQueryableAsync();
        var resp = customerQueryable.Select(s => new DropdownDto(s.Id.ToString().ToLower(), s.DisplayName)).ToList();
        return resp;
    }
}
