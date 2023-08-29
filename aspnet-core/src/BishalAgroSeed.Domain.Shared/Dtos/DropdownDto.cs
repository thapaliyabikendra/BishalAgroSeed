namespace BishalAgroSeed.Dtos;
public class DropdownDto
{
    public DropdownDto()
    {
    }

    public DropdownDto(string value, string name)
    {
        Value = value;
        Name = name;
    }

    public string Value { get; set; }
    public string Name { get; set; }
}
