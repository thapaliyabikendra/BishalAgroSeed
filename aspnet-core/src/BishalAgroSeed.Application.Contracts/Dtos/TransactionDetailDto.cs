using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BishalAgroSeed.Dtos;
public class TransactionDetailDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Cases { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
