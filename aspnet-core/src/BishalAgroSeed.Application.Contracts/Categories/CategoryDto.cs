using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace BishalAgroSeed.Categories;
public class CategoryDto : AuditedEntityDto<Guid>
{
    public string DisplayName{ get; set; }
    public Guid? ParentId { get; set; }
    public string ParentName { get; set; }
    public bool IsActive{ get; set; }
}
