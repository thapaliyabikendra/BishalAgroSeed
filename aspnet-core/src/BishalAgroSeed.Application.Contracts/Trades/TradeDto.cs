using System;

namespace BishalAgroSeed.Trades;
public class TradeDto
{
    public Guid TransactionId { get; set; }
    public Guid TradeTypeId { get; set; }
    public string TradeTypeName { get; set; }
    public string CustomerName { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TransportCharge { get; set; }
    public string VoucherNo { get; set; }
    public DateTime TranDate { get; set; }
    public decimal Amount { get; set; }
}
