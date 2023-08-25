using BishalAgroSeed.Transactions;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace BishalAgroSeed.TransactionPayments;
public class TransactionPayment : FullAuditedAggregateRoot<Guid>
{
    public TransactionPayment() { }
    public TransactionPayment(Guid id, Guid transactionId, PaymentTypes.PaymentType paymentTypeId, string? chequeNo, string? bankName, DateTime paidDate) : base(id)
    {
        TransactionId = transactionId;
        PaymentTypeId = paymentTypeId;
        ChequeNo = chequeNo;
        BankName = bankName;
        PaidDate = paidDate;

    }
    public Guid TransactionId { get; set; }
    [ForeignKey("TrasactionId")]
    public virtual Transaction Transaction { get; set; }
    public PaymentTypes.PaymentType PaymentTypeId { get; set; }
    public string? ChequeNo { get; set; }
    public string? BankName { get; set; }
    public DateTime PaidDate { get; set; }
}

