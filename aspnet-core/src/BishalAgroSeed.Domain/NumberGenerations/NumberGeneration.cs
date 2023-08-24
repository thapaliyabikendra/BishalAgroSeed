using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace BishalAgroSeed.NumberGenerations;
public class NumberGeneration:FullAuditedAggregateRoot<Guid>
{
    public NumberGeneration() { }
    public NumberGeneration (Guid id, string prefix, int number, string suffix, NumberGenerationTypes.NumberGenerationTypes numberGenerationTypeId) : base(id)
    {
        Prefix = prefix;
        Number = number;
        Suffix = suffix;
        NumberGenerationTypeId = numberGenerationTypeId;
         
    
    }
    public string Prefix { get; set; }
    public int Number { get; set; }
    public string Suffix { get; set; }
    public NumberGenerationTypes.NumberGenerationTypes NumberGenerationTypeId { get; set; }
}

