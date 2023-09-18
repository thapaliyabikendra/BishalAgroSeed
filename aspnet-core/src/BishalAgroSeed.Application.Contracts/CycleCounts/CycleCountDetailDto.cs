using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace BishalAgroSeed.CycleCounts;
public class CycleCountDetailDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public Guid CycleCountId { get; set; }
    public int SystemQuantity { get; set; }
    public int? PhysicalQuantity { get; set; }
    public string? Remarks { get; set; }
}