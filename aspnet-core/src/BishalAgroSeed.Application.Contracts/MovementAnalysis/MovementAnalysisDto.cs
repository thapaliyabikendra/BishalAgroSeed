using System;
using System.Collections.Generic;
using System.Text;

namespace BishalAgroSeed.MovementAnalysis;
public class MovementAnalysisDto
{
    public string Particulars { get; set; }
    public TradeMADto Purchases { get; set; }
    public TradeMADto Sales { get; set; }
    public DateTime TranDate { get; set; }
}
