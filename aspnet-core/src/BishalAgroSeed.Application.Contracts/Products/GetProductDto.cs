using System;

namespace BishalAgroSeed.Products;
public class GetProductDto
{
    public Guid Id { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
}