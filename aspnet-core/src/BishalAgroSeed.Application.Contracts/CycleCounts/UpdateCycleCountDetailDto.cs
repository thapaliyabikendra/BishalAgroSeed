using ExcelMapper;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BishalAgroSeed.CycleCounts;
public class UpdateCycleCountDetailDto
{
    [JsonIgnore]
    [ExcelIgnore]
    public int SN { get; set; }

    [ExcelIgnore]
    public Guid? Id { get; set; }

    [JsonIgnore]
    [ExcelColumnName("Product Name")]
    public string? ProductName { get; set; }

    [JsonIgnore]
    [ExcelColumnName("Physical Quantity")]
    public string? PhysicalQuantityName { get; set; }

    [ExcelIgnore]
    public int? PhysicalQuantity { get; set; }

    [ExcelColumnName("Remarks")]
    public string? Remarks { get; set; }
}