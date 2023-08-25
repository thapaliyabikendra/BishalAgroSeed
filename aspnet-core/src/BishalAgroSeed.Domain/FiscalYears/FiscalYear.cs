using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace BishalAgroSeed.FiscalYears;
public class FiscalYear : FullAuditedAggregateRoot<Guid>
{
    public FiscalYear() { }
    public FiscalYear(Guid id, string displayName, DateTime fromDate, DateTime toDate, bool isCurrent) : base(id)
    {
        DisplayName = displayName;
        FromDate = fromDate;
        ToDate = toDate;
        IsCurrent = isCurrent;
    }

    public string DisplayName { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public bool IsCurrent { get; set; }


}
