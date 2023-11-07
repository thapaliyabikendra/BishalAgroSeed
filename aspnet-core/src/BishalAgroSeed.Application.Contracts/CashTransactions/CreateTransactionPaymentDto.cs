namespace BishalAgroSeed.CashTransactions;
public class CreateTransactionPaymentDto
{
    public PaymentTypes.PaymentType? PaymentTypeId { get; set; }
    public string? ChequeNo { get; set; }
    public string? BankName { get; set; }
}
