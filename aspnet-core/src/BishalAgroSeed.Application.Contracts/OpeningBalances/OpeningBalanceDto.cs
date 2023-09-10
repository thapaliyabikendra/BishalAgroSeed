using System;
using Volo.Abp.Application.Dtos;

namespace BishalAgroSeed.OpeningBalances;
public class OpeningBalanceDto : AuditedEntityDto<Guid>
{
    public decimal Amount { get; set; }
    public DateTime TranDate { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; }
    public bool IsReceivable { get; set; }
}
