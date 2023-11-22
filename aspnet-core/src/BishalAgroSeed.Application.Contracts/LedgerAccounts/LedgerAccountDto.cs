using Microsoft.VisualBasic;
using System;

namespace BishalAgroSeed.LedgerAccounts;

public class LedgerAccountDto
{
    public string Miti { get; set; }
    public DateTime Date { get; set; }
    public string Particulars { get; set; }
    public string VchType { get; set; }
    public string VchNo {get; set;}
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public decimal Balance { get; set; }
}