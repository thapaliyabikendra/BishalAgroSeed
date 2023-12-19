using System;

namespace BishalAgroSeed.MovementAnalysis;
public class MovementAnalysisDto
{
    public string FromTranMiti { get; set; }
    public DateTime FromTranDate { get; set; }
    public string ToTranMiti { get; set; }
    public DateTime ToTranDate { get; set; }
    public string Particulars { get; set; }
    public TradeMADto Purchases { get; set; }
    public TradeMADto Sales { get; set; }
}
