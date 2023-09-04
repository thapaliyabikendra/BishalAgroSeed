using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace BishalAgroSeed.Products;

public class CreateUpdateProductDto
{
    [Required]
    public string DisplayName { get; set; }
    [Required]
    public Guid CategoryId { get; set; }
    [Required]
    public Guid BrandId { get; set; }
    [Required]
    public decimal Unit { get; set; }
    [Required]
    public Guid UnitTypeId { get; set; }
    [Required]
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public IFormFile? File { get; set; }
}