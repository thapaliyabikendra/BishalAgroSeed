using System;
using System.ComponentModel.DataAnnotations;

namespace BishalAgroSeed.CycleCounts;

public class CycleCountDetailFilter
{
    [Required]
    public Guid CycleCountId { get; set; }
    public string? CategoryName { get; set; }
    public string? ProductName { get; set; }
    public string? Remarks { get; set; }
}