using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace BishalAgroSeed.TranscationTypes;
public class TransactionType : Entity<Guid>
{
    public TransactionType() { }

    public TransactionType(Guid id, string displayName, string description):base(id)
    {
        DisplayName = displayName;
        Description = description;
    }

    public string DisplayName { get; set; }
    public string Description { get; set; }
}
