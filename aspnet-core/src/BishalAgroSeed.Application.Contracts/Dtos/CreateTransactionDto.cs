using BishalAgroSeed.CashTransactions;
using System;
using System.Collections.Generic;

namespace BishalAgroSeed.Dtos;
public class CreateTransactionDto
{
    public Guid? CustomerId { get; set; }
    public decimal? Amount { get; set; }
    public Guid? TransactionTypeId { get; set; }
    public decimal? DiscountAmount { get; set; } = 0;
    public decimal? TransportCharge { get; set; } = 0;
    public string? VoucherNo { get; set; }
    public DateTime? TranDate { get; set; }
    public CreateTransactionPaymentDto? Payment { get; set; }
    public List<CreateTransactionDetailDto>? Details { get; set; }
}