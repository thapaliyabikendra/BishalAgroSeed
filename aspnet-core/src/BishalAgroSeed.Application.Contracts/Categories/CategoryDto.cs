using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace BishalAgroSeed.Categories;
public class CategoryDto : AuditedEntityDto<Guid>
{
    public CategoryDto()
    {
    }

    public CategoryDto(string displayName, Guid? parentId, string parentName, bool isActive)
    {
        DisplayName = displayName;
        ParentId = parentId;
        ParentName = parentName;
        IsActive = isActive;
    }

    public string DisplayName{ get; set; }
    public Guid? ParentId { get; set; }
    public string ParentName { get; set; }
    public bool IsActive{ get; set; }
}
