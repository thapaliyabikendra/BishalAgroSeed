using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace BishalAgroSeed.Configurations;
public class Configuration : FullAuditedAggregateRoot<Guid>
{
    public Configuration() { }
    public Configuration(Guid id, string key, string value, string? description): base(id) {
        Key = key;
        Value = value;
        Description = description;
    }
    public string Key { get; set; }
    public string Value { get; set; }
    public string? Description { get; set; }
}
