using BishalAgroSeed.CycleCountNumbers;
using BishalAgroSeed.Products;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace BishalAgroSeed.CycleCounts;
public class CycleCount : FullAuditedAggregateRoot<Guid>
{
    public CycleCount() { }
    public CycleCount(Guid id, Guid productId, Guid cycleCountNumberId, decimal systemQuantity, decimal? physicalQuantity, string? remarks) : base(id)
    {
        ProductId = productId;
        CycleCountNumberId = cycleCountNumberId;
        SystemQuantity = systemQuantity;
        PhysicalQuantity = physicalQuantity;
        Remarks = remarks;
    }
    public Guid ProductId { get; set; }
    [ForeignKey("ProductId")]
    public virtual Product Product { get; set; }
    public Guid CycleCountNumberId { get; set; }
    [ForeignKey("CycleCountNumberId")]
    public CycleCountNumber CycleCountNumber { get; set; }
    public decimal SystemQuantity { get; set; }
    public decimal? PhysicalQuantity { get; set; }
    public string? Remarks { get; set; }
}