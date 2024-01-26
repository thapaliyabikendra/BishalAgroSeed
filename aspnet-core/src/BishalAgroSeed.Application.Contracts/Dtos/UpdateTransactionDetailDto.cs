using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BishalAgroSeed.Dtos;
public class UpdateTransactionDetailDto
{
    public Guid? Id { get; set; }
    [Required]
    public Guid ProductId { get; set; }
    [Required]
    public int Cases { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required]
    public decimal Price { get; set; }
}
