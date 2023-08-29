using System.ComponentModel.DataAnnotations;

namespace BishalAgroSeed.Brands;
public class CreateUpdateBrandDto
{
    [Required]
    public string DisplayName { get; set; }
    public string? Address { get; set; }
    public string? ContactNo { get; set; }
}
