using BishalAgroSeed.Brands;
using BishalAgroSeed.Categories;
using BishalAgroSeed.UnitTypes;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace BishalAgroSeed.Products;
public class Product : FullAuditedAggregateRoot<Guid>
{
    public Product() { }
    public Product(Guid id, string displayName, Guid categoryId, Guid brandId, decimal unit, Guid unitTypeId, decimal price, string? description, string? imgFileName) : base(id)
    {
        Id = id;
        DisplayName = displayName;
        CategoryId = categoryId;
        BrandId = brandId;
        Unit = unit;
        UnitTypeId = unitTypeId;
        Price = price;
        Description = description;
        ImgFileName = imgFileName;
    }

    public Product(string displayName, Guid categoryId, Category category, Guid brandId, Brand brand, decimal unit, Guid unitTypeId, UnitType unitType, decimal price, string? description, string? imgFileName)
    {
        DisplayName = displayName;
        CategoryId = categoryId;
        Category = category;
        BrandId = brandId;
        Brand = brand;
        Unit = unit;
        UnitTypeId = unitTypeId;
        UnitType = unitType;
        Price = price;
        Description = description;
        ImgFileName = imgFileName;
    }

    public string DisplayName { get; set; }
    public Guid CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public virtual Category Category { get; set; }

    public Guid BrandId { get; set; }
    [ForeignKey("BrandId")]
    public virtual Brand Brand { get; set; }
    public decimal Unit { get; set; }
    public Guid UnitTypeId { get; set; }
    [ForeignKey("UnitTypeId")]
    public virtual UnitType UnitType { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string? ImgFileName { get; set; }
}
