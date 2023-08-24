using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace BishalAgroSeed.UnitTypes;
public class UnitType : Entity<Guid>
{
    public UnitType() { }
    public UnitType(Guid id, string displayName, string description):base(id)
    {
        Id = id;
        DisplayName = displayName;
        Description = description;
    }
    public string DisplayName { get; set; }
    public string Description { get; set; }
}
