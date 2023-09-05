using BishalAgroSeed.Brands;
using BishalAgroSeed.Categories;
using BishalAgroSeed.Containers;
using BishalAgroSeed.Dtos;
using BishalAgroSeed.Permissions;
using BishalAgroSeed.UnitTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Validation;

namespace BishalAgroSeed.Products;
[Authorize(BishalAgroSeedPermissions.Products.Default)]
public class ProductAppService : CrudAppService<Product, ProductDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateProductDto>, IProductAppService
{
    private readonly IRepository<UnitType, Guid> _unitTypeRepository;
    private readonly IRepository<Category, Guid> _categoryRepository;
    private readonly IRepository<Brand, Guid> _brandRepository;
    private readonly IBlobContainer<ProductImageBlobContainer> _blobContainer;

    public ProductAppService(IRepository<Product, Guid> repository,
        IRepository<UnitType, Guid> unitTypeRepository,
        IRepository<Category, Guid> categoryRepository,
        IRepository<Brand, Guid> brandRepository,
        IBlobContainer<ProductImageBlobContainer> blobContainer
        ) : base(repository)
    {
        GetPolicyName = BishalAgroSeedPermissions.Products.Default;
        GetListPolicyName = BishalAgroSeedPermissions.Products.Create;
        CreatePolicyName = BishalAgroSeedPermissions.Products.Edit;
        CreatePolicyName = BishalAgroSeedPermissions.Products.Delete;
        _unitTypeRepository = unitTypeRepository;
        _categoryRepository = categoryRepository;
        _brandRepository = brandRepository;
        _blobContainer = blobContainer;
    }

    public override async Task<ProductDto> CreateAsync([FromForm] CreateUpdateProductDto input)
    {
        var result = await base.CreateAsync(input);
        if (input.File != null)
        {
            using (var memoryStream = new MemoryStream())
            {
                await input.File.CopyToAsync(memoryStream);
                await _blobContainer.SaveAsync(result.Id.ToString(), memoryStream.ToArray(), true);
            }
        }
        return result;
    }

    public override async Task<ProductDto> UpdateAsync(Guid id, [FromForm] CreateUpdateProductDto input)
    {
        var result = await base.UpdateAsync(id, input);
        if (input.File != null)
        {
            using (var memoryStream = new MemoryStream())
            {
                await input.File.CopyToAsync(memoryStream);
                await _blobContainer.SaveAsync(result.Id.ToString(), memoryStream.ToArray(), true);
            }
        }
        return result;
    }

    public async override Task<PagedResultDto<ProductDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        if (string.IsNullOrWhiteSpace(input.Sorting))
        {
            input.Sorting = $"DisplayName";
        }
        var _products = await Repository.GetQueryableAsync();
        var _categories = await _categoryRepository.GetQueryableAsync();
        var _brands = await _brandRepository.GetQueryableAsync();
        var _unitType = await _unitTypeRepository.GetQueryableAsync();
        var queryable = (
                        from p in _products
                        join c in _categories on p.CategoryId equals c.Id
                        join b in _brands on p.BrandId equals b.Id
                        join u in _unitType on p.UnitTypeId equals u.Id
                        select new ProductDto
                        {
                            Id = p.Id,
                            DisplayName = p.DisplayName,
                            CategoryId = p.CategoryId,
                            BrandId = p.BrandId,
                            CategoryName = c.DisplayName,
                            BrandName = b.DisplayName,
                            UnitTypeName = u.DisplayName,
                            UnitTypeDescription = u.Description,
                            Unit = p.Unit,
                            UnitTypeId = p.UnitTypeId,
                            Price = p.Price,
                            ImgFileName = p.ImgFileName
                        });

        var totalCount = queryable.Count();
        var data = queryable.Skip(input.SkipCount).Take(input.MaxResultCount).OrderBy(input.Sorting).ToList();
        return new PagedResultDto<ProductDto>(totalCount, data);
    }
    [Authorize(BishalAgroSeedPermissions.Products.Default)]
    public async Task<FileBlobDto> GetProductImageAsync(Guid id)
    {
        var result = await Repository.FirstOrDefaultAsync(s => s.Id == id);
        if (result == null)
        {
            var msg = "Product Not Found!!";
            throw new AbpValidationException(msg, new List<ValidationResult>() {
                new ValidationResult(msg, new []{ "id" })
            });
        }

        if (!(await _blobContainer.ExistsAsync(id.ToString())))
        {
            var msg = "Product Image Not Found!!";
            throw new AbpValidationException(msg, new List<ValidationResult>() {
                new ValidationResult(msg, new []{ "fileName" })
            });
        }

        var img = await _blobContainer.GetAllBytesAsync(id.ToString());
        var resp = new FileBlobDto(img, result.ImgFileName);
        return resp;
    }

    [Authorize(BishalAgroSeedPermissions.UnitTypes.Default)]
    public async Task<List<GetUnitTypeDto>> GetUnitTypesAsync()
    {
        var unitTypeQueryable = await _unitTypeRepository.GetQueryableAsync();
        var resp = unitTypeQueryable.Select(s => new GetUnitTypeDto { Id = s.Id, DisplayName = s.DisplayName, Description = s.Description }).ToList();
        return resp;
    }
    public async override Task DeleteAsync(Guid id)
    {
        var result = await Repository.FirstOrDefaultAsync(s => s.Id == id);
        if (result == null)
        {
            var msg = "Product Not Found!!";
            throw new AbpValidationException(msg, new List<ValidationResult>() {
                new ValidationResult(msg, new []{ "id" })
            });
        }
        await base.DeleteAsync(id);
        await _blobContainer.DeleteAsync(id.ToString());
    }

    public async override Task<ProductDto> GetAsync(Guid id)
    {
        var _products = await Repository.GetQueryableAsync();
        var _unitType = await _unitTypeRepository.GetQueryableAsync();
        var data = (
                        from p in _products
                        join u in _unitType on p.UnitTypeId equals u.Id
                        where p.Id == id
                        select new ProductDto
                        {
                            Id = p.Id,
                            DisplayName = p.DisplayName,
                            CategoryId = p.CategoryId,
                            BrandId = p.BrandId,
                            UnitTypeDescription = u.Description,
                            Unit = p.Unit,
                            UnitTypeId = p.UnitTypeId,
                            Price = p.Price,
                            ImgFileName = p.ImgFileName
                        }).FirstOrDefault();

        return data;
    }
}
