using System.ComponentModel;

namespace BishalAgroSeed.Constants;
public class TransactionTypes
{
    [Description("Trade")]
    public static readonly string PURCHASE = "Purchase";

    [Description("Cash Transaction")]
    public static readonly string CASH_Payment = "Cash Payment";

    [Description("Trade")]
    public static readonly string SALES = "Sales";

    [Description("Cash Transaction")]
    public static readonly string CASH_RECEIPT = "Cash Receipt";
}
