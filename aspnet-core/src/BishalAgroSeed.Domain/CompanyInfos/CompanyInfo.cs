using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace BishalAgroSeed.CompanyInfos;
public class CompanyInfo : FullAuditedAggregateRoot<Guid>
{
    public CompanyInfo() { }
    public CompanyInfo(Guid id, string displayName, string address, string contactNo, string panNo) : base(id)
    {
        DisplayName = displayName;
        Address = address;
        ContactNo = contactNo;
        PanNo = panNo;
    }
    public string DisplayName { get; set; }
    public string Address { get; set; }
    public string ContactNo { get; set; }
    public string PanNo { get; set; }
}


