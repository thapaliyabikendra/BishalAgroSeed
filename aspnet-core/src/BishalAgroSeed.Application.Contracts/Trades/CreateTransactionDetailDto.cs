using System;
using System.ComponentModel.DataAnnotations;

namespace BishalAgroSeed.Trades;
public class CreateTransactionDetailDto
{
    [Required]
    public Guid ProductId { get; set; }
    [Required]
    public int Cases { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required]
    public decimal Price { get; set; }
}