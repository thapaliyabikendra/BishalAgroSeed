using System;

namespace BishalAgroSeed.CycleCounts;
public class CycleCountDto
{
    public Guid Id { get; set; } 
    public string CCINumber { get; set; }
    public bool IsClosed { get; set; }
    public DateTime? ClosedDate { get; set; }
    public string ClosedByName { get; set; }
    public DateTime CreationTime { get; set; }
}