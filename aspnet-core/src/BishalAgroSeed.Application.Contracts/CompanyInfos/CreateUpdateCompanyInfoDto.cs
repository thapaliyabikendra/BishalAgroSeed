using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BishalAgroSeed.CompanyInfos;
public class CreateUpdateCompanyInfoDto
{
    [Required]
    public string DisplayName { get; set; }
    public string Address { get; set; }
    public string ContactNo { get; set; }
    public string PanNo { get; set; }
}
