namespace BishalAgroSeed.NumberGenerations;

public class CreateUpdateNumberGenerationDto
{
    public string Prefix { get; set; }
    public int Number { get; set; }
    public string Suffix { get; set; }
    public NumberGenerationTypes.NumberGenerationTypes NumberGenerationTypeId { get; set; }
}