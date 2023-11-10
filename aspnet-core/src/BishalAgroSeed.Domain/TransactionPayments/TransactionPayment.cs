using BishalAgroSeed.Transactions;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace BishalAgroSeed.TransactionPayments;
public class TransactionPayment : FullAuditedAggregateRoot<Guid>
{
    public TransactionPayment() { }
    public TransactionPayment(Guid id, Guid transactionId, PaymentTypes.PaymentType paymentTypeId, string? chequeNo, string? bankName) : base(id)
    {
        TransactionId = transactionId;
        PaymentTypeId = paymentTypeId;
        ChequeNo = chequeNo;
        BankName = bankName;
    }
    public Guid TransactionId { get; set; }
    [ForeignKey("TransactionId")]
    public virtual Transaction Transaction { get; set; }
    public PaymentTypes.PaymentType PaymentTypeId { get; set; }
    public string? ChequeNo { get; set; }
    public string? BankName { get; set; }
}