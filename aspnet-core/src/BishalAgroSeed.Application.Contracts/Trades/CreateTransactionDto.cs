using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BishalAgroSeed.Trades;
public class CreateTransactionDto
{
    public Guid? CustomerId { get; set; }
    public decimal? Amount { get; set; }
    public Guid? TransactionTypeId { get; set; }
    public decimal? DiscountAmount { get; set; } = 0;
    public decimal? TransportCharge { get; set; } = 0;
    public string? VoucherNo { get; set; }
    public List<CreateTransactionDetailDto>? Details { get; set; } 
}