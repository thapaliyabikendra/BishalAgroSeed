using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BishalAgroSeed.Services;
public interface IExcelService
{
    public Task<byte[]> ExportAsync<TData>(List<TData> data,
     Dictionary<string, Func<TData, object>> mappers,
     string sheetName = "Sheet1");
}