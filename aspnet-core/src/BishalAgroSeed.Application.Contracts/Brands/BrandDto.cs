using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace BishalAgroSeed.Brands;
public class BrandDto:AuditedEntityDto<Guid>
{
    public string DisplayName { get; set; }
    public string Address { get; set; }
    public string ContactNo { get; set; }    
}
