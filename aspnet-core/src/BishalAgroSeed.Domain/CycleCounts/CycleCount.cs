using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace BishalAgroSeed.CycleCounts;
public class CycleCount: FullAuditedAggregateRoot<Guid>
{
    public CycleCount()
    {
    }

    public CycleCount(Guid id, string cciNumber, bool isClosed, DateTime? closedDate, Guid? closedBy, IdentityUser user): base(id)
    {
        CCINumber = cciNumber;
        IsClosed = isClosed;
        ClosedDate = closedDate;
        ClosedBy = closedBy;
        User = user;
    }

    public string CCINumber { get; set; }
    public bool IsClosed { get; set; }
    public DateTime? ClosedDate { get; set; }
    public Guid? ClosedBy { get; set; }
    [ForeignKey("CloseBy")]
    public IdentityUser User { get; set; }
}
