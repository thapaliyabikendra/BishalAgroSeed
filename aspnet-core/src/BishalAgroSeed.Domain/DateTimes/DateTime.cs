using System;
using Volo.Abp.Domain.Entities;

namespace BishalAgroSeed.DateTimes;
public class DateTime : Entity<Guid>
{
    public DateTime() { }
    public DateTime(Guid id, System.DateTime datetime, string dateTimeNepali): base(id)
    {
        Datetime = datetime;
        DatetimeNepali = dateTimeNepali;
    }
    public System.DateTime Datetime { get; set; }
    public string DatetimeNepali { get; set; }
}

