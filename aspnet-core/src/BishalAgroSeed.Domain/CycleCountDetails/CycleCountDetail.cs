using BishalAgroSeed.CycleCounts;
using BishalAgroSeed.Products;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace BishalAgroSeed.CycleCountDetails;
public class CycleCountDetail : FullAuditedAggregateRoot<Guid>
{
    public CycleCountDetail() { }
    public CycleCountDetail(Guid id, Guid productId, Guid cycleCountId, int systemQuantity, int? physicalQuantity, string? remarks) : base(id)
    {
        ProductId = productId;
        CycleCountId = cycleCountId;
        SystemQuantity = systemQuantity;
        PhysicalQuantity = physicalQuantity;
        Remarks = remarks;
    }
    public Guid ProductId { get; set; }
    [ForeignKey("ProductId")]
    public virtual Product Product { get; set; }
    public Guid CycleCountId { get; set; }
    [ForeignKey("CycleCountId")]
    public CycleCount CycleCount { get; set; }
    public int SystemQuantity { get; set; }
    public int? PhysicalQuantity { get; set; }
    public string? Remarks { get; set; }
}