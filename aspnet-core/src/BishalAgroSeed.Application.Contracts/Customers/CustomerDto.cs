using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace BishalAgroSeed.Customers;
public class CustomerDto:AuditedEntityDto<Guid>
{
    public string DisplayName { get; set; }
    public string Address { get; set; }
    public string ContactNo { get; set; }
    public bool IsVendor { get; set; }
    public bool IsCustomer { get; set; }
}
