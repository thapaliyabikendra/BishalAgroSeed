using System;
using Volo.Abp.Application.Dtos;

namespace BishalAgroSeed.Products;
public class ProductDto : AuditedEntityDto<Guid>
{
    public string DisplayName { get; set; }

    public Guid BrandId { get; set; }
    public decimal Unit { get; set; }
    public Guid UnitTypeId { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
}
