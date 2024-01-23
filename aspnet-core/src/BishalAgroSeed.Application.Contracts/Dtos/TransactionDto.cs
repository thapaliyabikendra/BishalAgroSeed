using System;
using System.Collections.Generic;
using System.Text;

namespace BishalAgroSeed.Dtos;
public class TransactionDto
{
    public Guid Id { get; set; }
    public Guid? CustomerId { get; set; }
    public decimal? Amount { get; set; }
    public Guid? TransactionTypeId { get; set; }
    public decimal? DiscountAmount { get; set; } 
    public decimal? TransportCharge { get; set; } 
    public string? VoucherNo { get; set; }
    public DateTime? TranDate { get; set; }
    public List<TransactionDetailDto>? Details { get; set; }
}
