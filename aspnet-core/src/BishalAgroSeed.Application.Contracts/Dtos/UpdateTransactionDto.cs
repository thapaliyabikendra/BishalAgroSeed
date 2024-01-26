using System;
using System.Collections.Generic;
using System.Text;

namespace BishalAgroSeed.Dtos;
public class UpdateTransactionDto
{
    public Guid Id { get; set; }
    public Guid? CustomerId { get; set; }
    public decimal? Amount { get; set; }
    public Guid? TransactionTypeId { get; set; }
    public decimal? DiscountAmount { get; set; } = 0;
    public decimal? TransportCharge { get; set; } = 0;
    public string? VoucherNo { get; set; }
    public DateTime? TranDate { get; set; }
    public UpdateTransactionPaymentDto? Payment { get; set; }
    public List<UpdateTransactionDetailDto>? Details { get; set; }
}
