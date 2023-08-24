using BishalAgroSeed.Products;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace BishalAgroSeed.InventoryCounts;
public class InventoryCount : FullAuditedAggregateRoot<Guid>
{
    public InventoryCount() { }
    public InventoryCount(Guid id, Guid productId, int count, DateTime countDate) : base(id)
    {
        ProductId = productId;
        Count = count;
        CountDate = countDate;
    }
    public Guid ProductId { get; set; }
    [ForeignKey("ProductId")]
    public virtual Product Product { get; set; }
    public int Count { get; set; }
    public DateTime CountDate { get; set; }
}
