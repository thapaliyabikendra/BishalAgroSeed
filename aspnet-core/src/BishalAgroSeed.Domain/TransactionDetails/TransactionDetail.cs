using BishalAgroSeed.Products;
using BishalAgroSeed.Transactions;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace BishalAgroSeed.TransactionDetails;
public class TransactionDetail : FullAuditedAggregateRoot<Guid>
{
    public TransactionDetail() { }
    public TransactionDetail(Guid id, Guid transactionId, Guid productId, decimal cases, int quantity, decimal price) : base(id)
    {
        TransactionId = transactionId;
        ProductId = productId;
        Cases = cases;
        Quantity = quantity;
        Price = price;

    }
    public Guid TransactionId { get; set; }
    [ForeignKey("TransactionId")]
    public virtual Transaction Transaction { get; set; }
    public Guid ProductId { get; set; }
    [ForeignKey("ProductId")]
    public virtual Product Product { get; set; }
    public decimal Cases { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
