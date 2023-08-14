using System.ComponentModel.DataAnnotations;

namespace BishalAgroSeed.Configurations;

public class CreateUpdateConfigurationDto
{
    [Required]
    public string Key { get; set; }
    [Required]
    public string Value { get; set; }
    public string Description { get; set; }
}