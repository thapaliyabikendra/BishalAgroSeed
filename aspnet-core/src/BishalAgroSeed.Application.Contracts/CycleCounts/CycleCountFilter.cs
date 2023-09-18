using System;

namespace BishalAgroSeed.CycleCounts;

public class CycleCountFilter
{
    public string? CCINumber { get; set; }
    public bool? IsClosed { get; set; }
    public DateTime? ClosedFromDate { get; set; }
    public DateTime? ClosedToDate { get; set; }
    public string? ClosedByName { get; set; }
    public DateTime? OpenedFromDate { get; set; }
    public DateTime? OpenedToDate { get; set; }
}