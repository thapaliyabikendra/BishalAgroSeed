using BishalAgroSeed.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace BishalAgroSeed.Customers;
public interface ICustomerAppService: ICrudAppService<CustomerDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateCustomerDto>
{
    public Task<List<DropdownDto>> GetCustomersAsync();

}
