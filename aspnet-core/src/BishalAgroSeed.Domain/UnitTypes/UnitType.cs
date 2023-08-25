using System;
using Volo.Abp.Domain.Entities;

namespace BishalAgroSeed.UnitTypes;
public class UnitType : Entity<Guid>
{
    public UnitType() { }
    public UnitType(Guid id, string displayName, string? description) : base(id)
    {
        Id = id;
        DisplayName = displayName;
        Description = description;
    }
    public string DisplayName { get; set; }
    public string? Description { get; set; }
}
