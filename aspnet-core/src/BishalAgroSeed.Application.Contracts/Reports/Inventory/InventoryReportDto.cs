using System;
using System.Collections.Generic;
using System.Text;

namespace BishalAgroSeed.Reports.Inventory;
public class InventoryReportDto
{
    public string ProductName { get; set;}
    public int Count { get; set;}
    public DateTime CountDate { get; set;}
}
