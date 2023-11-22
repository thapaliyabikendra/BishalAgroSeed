using System;
using System.ComponentModel.DataAnnotations;

namespace BishalAgroSeed.LedgerAccounts;

public class LedgerAccountFilter
{
    [Required]
    public Guid CustomerId { get; set; }
    [Required]
    public DateTime FromTranDate { get; set; } = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
    [Required]
    public DateTime ToTranDate { get; set; } = DateTime.Today;
}