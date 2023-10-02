using System;
using Volo.Abp.Application.Dtos;

namespace BishalAgroSeed.Configurations;

public class ConfigurationDto:AuditedEntityDto<Guid>
{
    public ConfigurationDto()
    {
    }

    public ConfigurationDto(string key, string value, string description)
    {
        Key = key;
        Value = value;
        Description = description;
    }

    public string Key { get; set; }
    public string Value { get; set; }
    public string Description { get; set; }
}