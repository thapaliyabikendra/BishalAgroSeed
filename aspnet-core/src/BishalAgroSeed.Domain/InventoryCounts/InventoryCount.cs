using BishalAgroSeed.Products;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace BishalAgroSeed.InventoryCounts;
public class InventoryCount : Entity<Guid>
{
    public InventoryCount() { }
    public InventoryCount(Guid id, Guid productId, int count, DateTime countDate)
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
