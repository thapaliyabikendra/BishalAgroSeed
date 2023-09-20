using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BishalAgroSeed.CycleCounts;
public class UpdateCycleCountDetailDto
{
    [JsonIgnore]
    public int SN { get; set; }
    public Guid? Id { get; set; }
    public int? PhysicalQuantity { get; set; }
    public string? Remarks { get; set; }
}