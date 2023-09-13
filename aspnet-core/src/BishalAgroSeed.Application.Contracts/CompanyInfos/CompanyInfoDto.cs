using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace BishalAgroSeed.CompanyInfos;
public class CompanyInfoDto:AuditedEntityDto<Guid>
{
    public string DisplayName { get; set; }
    public string? Address { get; set; }
    public string? ContactNo { get; set; }
    public string? PanNo { get; set; }
}