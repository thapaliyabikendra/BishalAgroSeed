using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BishalAgroSeed.Categories;
public class CreateUpdateCategoryDto
{
    [Required]
    public string DisplayName { get; set; }
    public Guid? ParentId { get; set; }
    [Required]
    public bool IsActive { get; set; }
}
