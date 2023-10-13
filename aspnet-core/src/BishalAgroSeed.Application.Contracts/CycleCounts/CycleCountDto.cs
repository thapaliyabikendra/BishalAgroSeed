using System;

namespace BishalAgroSeed.CycleCounts;
public class CycleCountDto
{
    public Guid Id { get; set; } 
    public string CCINumber { get; set; }
    public bool IsClosed { get; set; }
    public DateTime? ClosedAt { get; set; }
    public string? ClosedByName { get; set; }
    public DateTime RequestedAt { get; set; }
    public string? RequestedByName { get; set; }
}