using System.ComponentModel.DataAnnotations;

namespace BishalAgroSeed.Customers;

public class CreateUpdateCustomerDto
{
    [Required]
    public string DisplayName { get; set;}
    public string? Address { get; set;}
    public string? ContactNo { get; set;}
    public bool IsCustomer { get; set;}=true;
    public bool IsVendor { get; set;}=false;
}