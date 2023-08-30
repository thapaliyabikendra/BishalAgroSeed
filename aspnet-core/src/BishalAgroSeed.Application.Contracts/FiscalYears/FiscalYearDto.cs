using System;
using Volo.Abp.Application.Dtos;

namespace BishalAgroSeed.FiscalYears;
public class FiscalYearDto : FullAuditedEntityDto<Guid>
{
    public string DisplayName { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public bool IsCurrent { get; set; }

}
