using System.ComponentModel.DataAnnotations;

namespace BishalAgroSeed.NumberGenerations;

public class CreateUpdateNumberGenerationDto
{
    public string? Prefix { get; set; }
    [Required]
    public int Number { get; set; }
    public string? Suffix { get; set; }
    [Required]
    public NumberGenerationTypes.NumberGenerationTypes NumberGenerationTypeId { get; set; }
}