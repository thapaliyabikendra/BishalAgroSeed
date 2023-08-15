using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace BishalAgroSeed.Customers;
public class Customer : FullAuditedAggregateRoot<Guid>
{
    public Customer() { }
    public Customer(Guid id, string displayName, string address, string contactNo, bool isCustomer, bool isVendor) : base(id)
    {
        DisplayName = displayName;
        Address = address;
        ContactNo = contactNo;  
        IsCustomer = isCustomer;
        IsVendor = isVendor;
    }
    public string DisplayName { get; set; }
    public string Address { get; set; }
    public string ContactNo { get; set; }
    public bool IsCustomer { get; set; }
    public bool IsVendor { get; set; }
}
