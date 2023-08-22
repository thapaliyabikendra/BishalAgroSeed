using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace BishalAgroSeed.Categories;
public class Category : FullAuditedAggregateRoot<Guid>
{
    public Category() { }
    public Category(Guid id, string displayName, Guid parentId, bool isActive):base(id)
    {
        DisplayName = displayName;
        ParentId = parentId;
        IsActive = isActive;
    }
    public string DisplayName { get; set; }

    [ForeignKey("Parent")]
    public Guid? ParentId { get; set; }
    public virtual Category Parent { get; set; }

    public bool IsActive { get; set; }
}
