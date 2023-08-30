using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace BishalAgroSeed.OpeningBalances;

public class CreateUpdateOpeningBalanceDto
{
    public decimal Amount { get; set; }
    public DateTime TranDate { get; set; }
    public Guid CustomerId { get; set; }
   
    public bool IsReceivable { get; set; }
}