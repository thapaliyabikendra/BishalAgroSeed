using BishalAgroSeed.Customers;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace BishalAgroSeed.OpeningBalances;
public class OpeningBalance : FullAuditedAggregateRoot<Guid>
{
    public OpeningBalance() { }
    public OpeningBalance(Guid id, decimal amount, DateTime tranDate, Guid customerId, bool isReceivable) : base(id)
    {
        Amount = amount;
        TranDate = tranDate;
        CustomerId = customerId;
        IsReceivable = isReceivable;
    }
    public decimal Amount { get; set; }
    public DateTime TranDate { get; set; }
    public Guid CustomerId { get; set; }
    [ForeignKey("CustomerId")]
    public virtual Customer Customer { get; set; }
    public bool IsReceivable { get; set; }
}
