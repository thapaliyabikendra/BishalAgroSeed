using System;
using System.ComponentModel.DataAnnotations;

namespace BishalAgroSeed.FiscalYears;

public class CreateUpdateFiscalYearDto
{
    [Required]
    public string DisplayName { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public bool IsCurrent { get; set; }

}