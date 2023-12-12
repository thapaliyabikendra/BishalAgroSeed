using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BishalAgroSeed.MovementAnalysis;
public class MovementAnalysisFilter
{
    public string? ProductName { get; set; }
    [Required]
    public DateTime FromTranDate { get; set; } = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
    [Required]
    public DateTime ToTranDate { get; set; } = DateTime.Today;
}
