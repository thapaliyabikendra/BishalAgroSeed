using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BishalAgroSeed.Trades;
public class TradeFilter
{
    public Guid? TradeTypeId { get; set; }
    public Guid? CustomerId { get; set; }
    public string? VoucherNo { get; set; }
    public DateTime FromTranDate { get; set; }
    public DateTime ToTranDate { get; set; }

}
