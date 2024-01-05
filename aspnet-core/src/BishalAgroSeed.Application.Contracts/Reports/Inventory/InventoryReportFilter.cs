using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BishalAgroSeed.Reports.Inventory;
public class InventoryReportFilter
{
    public string? ProductName { get; set; }
    [Required]
    public DateTime CountDate { get; set; } = DateTime.Today;
}
