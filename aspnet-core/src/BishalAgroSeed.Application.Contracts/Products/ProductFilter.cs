using System;

namespace BishalAgroSeed.Products;

public class ProductFilter
{
    public string? DisplayName { get; set; }
    public string? CategoryName { get; set; }
    public string? BrandName { get; set; }
    public string? UnitTypeName { get; set; }
    public decimal? PriceFrom { get; set; }
    public decimal? PriceTo { get; set; }
    public string? Description { get; set; }
}