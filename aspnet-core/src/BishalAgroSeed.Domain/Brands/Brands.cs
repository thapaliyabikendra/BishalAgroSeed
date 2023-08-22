using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace BishalAgroSeed.Brands;
public class Brand:FullAuditedAggregateRoot<Guid>
{
    public Brand() { }
    public Brand(Guid id, string displayName,string address,string contactNo):base(id)
    {
        DisplayName = displayName;
        Address = address;
        ContactNo = contactNo;
    }
    public string DisplayName { get; set; }
    public string Address { get; set; }
    public string ContactNo { get; set; }
}
