using System;
using Volo.Abp.Application.Dtos;

namespace BishalAgroSeed.Products;
public class ProductDto : AuditedEntityDto<Guid>
{
    public string DisplayName { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }
    public Guid BrandId { get; set; }
    public string BrandName { get; set; }
    public decimal Unit { get; set; }
    public Guid UnitTypeId { get; set; }
    public string UnitTypeName { get; set; }
    public string UnitTypeDescription { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }

    public string ImgFileName { get; set; }
    public DateTime CreationTime { get; set; }

}
