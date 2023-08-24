using BishalAgroSeed.Customers;
using BishalAgroSeed.TranscationTypes;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace BishalAgroSeed.Transactions;
public class Transaction : FullAuditedAggregateRoot<Guid>
{
    public Transaction() { }
    public Transaction(Guid id, DateTime tranDate, Guid customerId, decimal amount, Guid transactionTypeId, decimal discountAmount, decimal transportCharge, string voucherNo) : base(id)
    {
        TranDate = tranDate;
        CustomerId = customerId;
        Amount = amount;
        TransactionTypeId = transactionTypeId;
        DiscountAmount = discountAmount;
        TransportCharge = transportCharge;
        VoucherNo = voucherNo;
    }
    public DateTime TranDate { get; set; }
    public Guid CustomerId { get; set; }
    [ForeignKey("CustomerId")]
    public virtual Customer Customer { get; set; }
    public decimal Amount { get; set; }
    public Guid TransactionTypeId { get; set; }
    [ForeignKey("TransactionTypeId")]
    public virtual TransactionType TransactionType { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TransportCharge { get; set; }
    public string VoucherNo { get; set; }
}
