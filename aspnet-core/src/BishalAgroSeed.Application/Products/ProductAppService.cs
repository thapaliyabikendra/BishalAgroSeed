using BishalAgroSeed.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace BishalAgroSeed.Products;
[Authorize(BishalAgroSeedPermissions.Products.Default)]
public class ProductAppService : CrudAppService<Product, ProductDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateProductDto>, IProductAppService
{
    public ProductAppService(IRepository<Product, Guid> repository) : base(repository)
    {
        GetPolicyName = BishalAgroSeedPermissions.Products.Default;
        GetListPolicyName = BishalAgroSeedPermissions.Products.Create;
        CreatePolicyName = BishalAgroSeedPermissions.Products.Edit;
        CreatePolicyName = BishalAgroSeedPermissions.Products.Delete;
    }

}
