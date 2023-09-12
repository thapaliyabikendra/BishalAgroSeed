using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace BishalAgroSeed.NumberGenerations;
public class NumberGenerationDto: AuditedEntityDto<Guid>
{
    public string Prefix { get; set; }
    public int Number { get; set; }
    public string Suffix { get; set; }
    public NumberGenerationTypes.NumberGenerationTypes NumberGenerationTypeId { get; set; }
    public string NumberGenerationTypeName { get; set; }
}
